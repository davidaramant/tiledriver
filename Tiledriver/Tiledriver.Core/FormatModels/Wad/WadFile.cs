// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Tiledriver.Core.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.Core.FormatModels.Wad
{
    public sealed class WadFile
    {
        private struct LumpMetadata
        {
            private readonly int _position;
            private readonly int _size;
            private readonly LumpName _name;

            public LumpMetadata(int position, int size, LumpName name)
            {
                _position = position;
                _size = size;
                _name = name;
            }

            public void WriteTo(Stream stream)
            {
                stream.WriteInt(_position);
                stream.WriteInt(_size);
                stream.WriteText(_name.ToString(), totalLength: LumpName.MaxLength);
            }
        }

        private readonly List<ILump> _lumps = new List<ILump>();

        public WadFile()
        {
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
                    int startOfLump = (int) fs.Position;

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
            throw new NotImplementedException();
        }


    }
}
