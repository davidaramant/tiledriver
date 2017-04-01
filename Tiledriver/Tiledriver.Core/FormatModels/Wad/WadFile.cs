// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Tiledriver.Core.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.Core.FormatModels.Wad
{
    // TODO: Should this be stateful to allow for lazy-loaded lumps?
    public sealed class WadFile : IEnumerable<ILump>
    {
        private struct LumpMetadata
        {
            public readonly int Position;
            public readonly int Size;
            public readonly LumpName Name;

            public LumpMetadata(int position, int size, LumpName name)
            {
                Position = position;
                Size = size;
                Name = name;
            }

            public void WriteTo(Stream stream)
            {
                stream.WriteInt(Position);
                stream.WriteInt(Size);
                stream.WriteText(Name.ToString(), totalLength: LumpName.MaxLength);
            }

            public static LumpMetadata ReadFrom(Stream stream)
            {
                return new LumpMetadata(
                    position: stream.ReadInt(),
                    size: stream.ReadInt(),
                    name: stream.ReadText(LumpName.MaxLength).TrimEnd((char)0));
            }
        }

        private readonly List<ILump> _lumps = new List<ILump>();

        public int Count => _lumps.Count;

        public ILump this[int index] => _lumps[index];

        public WadFile()
        {
        }

        private WadFile(IEnumerable<ILump> lumps)
        {
            _lumps.AddRange(lumps);
        }

        public void Append([NotNull]ILump lump)
        {
            _lumps.Add(lump);
        }

        public void SaveTo(Stream stream)
        {
            stream.WriteText("PWAD");
            stream.WriteInt(_lumps.Count);

            // Fill in this position after writing the data
            int directoryOffsetPosition = (int)stream.Position;
            stream.Position += 4;

            var metadata = new List<LumpMetadata>();
            foreach (var lump in _lumps)
            {
                int startOfLump = (int)stream.Position;

                lump.WriteTo(stream);

                metadata.Add(new LumpMetadata(
                    position: startOfLump,
                    size: (int)stream.Position - startOfLump,
                    name: lump.Name));
            }

            int startOfDirectory = (int)stream.Position;

            // Write directory
            foreach (var lumpMetadata in metadata)
            {
                lumpMetadata.WriteTo(stream);
            }

            // Go back and set the directory position
            stream.Position = directoryOffsetPosition;
            stream.WriteInt(startOfDirectory);
        }

        public void SaveTo(string filePath)
        {
            using (var fs = File.Open(filePath, FileMode.Create))
            {
                SaveTo(fs);
            }
        }

        public static WadFile Read(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            {
                return Read(fs);
            }
        }

        public static WadFile Read(Stream stream)
        {
            // TODO: More graceful handling of invalid files.

            var identification = stream.ReadText(4);
            var numLumps = stream.ReadInt();
            var directoryPosition = stream.ReadInt();

            stream.Position = directoryPosition;

            var directory =
                Enumerable.Range(1, numLumps).
                Select(_ => LumpMetadata.ReadFrom(stream));

            return new WadFile(directory.Select<LumpMetadata, ILump>(info =>
            {
                if (info.Size == 0)
                {
                    return new Marker(info.Name);
                }
                else
                {
                    stream.Position = info.Position;
                    return new DataLump(info.Name, stream.ReadArray(info.Size));
                }
            }));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ILump> GetEnumerator()
        {
            return _lumps.GetEnumerator();
        }
    }
}
