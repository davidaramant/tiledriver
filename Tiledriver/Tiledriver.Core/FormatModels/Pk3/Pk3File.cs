// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Archives.Zip;
using SharpCompress.Readers;

namespace Tiledriver.Core.FormatModels.Pk3
{
    public sealed class Pk3File : IResourceProvider, IDisposable
    {
        private readonly string _fileName;
        private readonly ZipArchive _archive;

        private Pk3File(string fileName)
        {
            _fileName = fileName;
            _archive = ZipArchive.Open(File.OpenRead(_fileName), new ReaderOptions { LeaveStreamOpen = false });
        }

        public static Pk3File Open(string fileName)
        {
            return new Pk3File(fileName);
        }

        public Stream Lookup(string path)
        {
            var entry = _archive.Entries.SingleOrDefault(e => !e.IsDirectory &&
                                                   StringComparer.InvariantCultureIgnoreCase.Equals(path, e.Key));
            if (entry == null)
            {
                throw new EntryNotFoundException(path);
            }

            return entry.OpenEntryStream();
        }

        public IEnumerator<string> GetAllEntryNames()
        {
            foreach (var entry in _archive.Entries.Where(entry => !entry.IsDirectory))
            {
                yield return entry.Key;
            }
        }

        public void Dispose()
        {
            _archive.Dispose();
        }
    }
}