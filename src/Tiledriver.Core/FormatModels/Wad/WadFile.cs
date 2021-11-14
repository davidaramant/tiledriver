// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.Core.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.Core.FormatModels.Wad
{
    // TODO: Should this be stateful to allow for lazy-loaded lumps?
    public sealed class WadFile : IEnumerable<ILump>
    {
        private readonly List<ILump> _lumps = new();
        public int Count => _lumps.Count;

        public ILump this[int index] => _lumps[index];

        private WadFile(IEnumerable<ILump> lumps)
        {
            _lumps.AddRange(lumps);
        }

        public static WadFile Read(string filePath)
        {
            using var fs = File.OpenRead(filePath);
            return Read(fs);
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
                Select(_ => LumpMetadata.ReadFrom(stream))
                .ToList();

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
