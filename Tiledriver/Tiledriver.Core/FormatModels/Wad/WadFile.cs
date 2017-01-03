// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Tiledriver.Core.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.Core.FormatModels.Wad
{
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

        public void SaveTo(string filePath)
        {
            using (var fs = File.Open(filePath, FileMode.Create))
            {
                fs.WriteText("PWAD");
                fs.WriteInt(_lumps.Count);

                // Fill in this position after writing the data
                int directoryOffsetPosition = (int)fs.Position;
                fs.Position += 4;

                var metadata = new List<LumpMetadata>();
                foreach (var lump in _lumps)
                {
                    int startOfLump = (int)fs.Position;

                    lump.WriteTo(fs);

                    metadata.Add(new LumpMetadata(
                        position: startOfLump,
                        size: (int)fs.Position - startOfLump,
                        name: lump.Name));
                }

                int startOfDirectory = (int)fs.Position;

                // Write directory
                foreach (var lumpMetadata in metadata)
                {
                    lumpMetadata.WriteTo(fs);
                }

                // Go back and set the directory position
                fs.Position = directoryOffsetPosition;
                fs.WriteInt(startOfDirectory);
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
            var identification = stream.ReadText(4);
            var numLumps = stream.ReadInt();
            var directoryPosition = stream.ReadInt();

            stream.Position = directoryPosition;

            var directory = new List<LumpMetadata>();
            for (int i = 0; i < numLumps; i++)
            {
                directory.Add(LumpMetadata.ReadFrom(stream));
            }

            var lumps = directory.Select<LumpMetadata, ILump>(info =>
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
             });

            return new WadFile(lumps);
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
