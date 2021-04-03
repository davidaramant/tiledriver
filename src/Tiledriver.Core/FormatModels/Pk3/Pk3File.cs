// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;
using SharpCompress.Archives.Zip;
using SharpCompress.Readers;

namespace Tiledriver.Core.FormatModels.Pk3
{
    public sealed class Pk3File : IResourceProvider
    {
        private readonly ZipArchive _archive;

        private Pk3File(string fileName)
        {
            _archive = ZipArchive.Open(File.OpenRead(fileName), new ReaderOptions { LeaveStreamOpen = false });
        }

        public static Pk3File Open(string fileName)
        {
            return new Pk3File(fileName);
        }

        public Stream Lookup(string path)
        {
            return TryLookup(path).OrElse(() => new EntryNotFoundException(path));
        }

        public Maybe<Stream> TryLookup(string path)
        {
            var entry = _archive.Entries.SingleOrDefault(e =>
                !e.IsDirectory && StringComparer.InvariantCultureIgnoreCase.Equals(path, e.Key));

            if (entry == null)
            {
                return Maybe<Stream>.Nothing;
            }

            return entry.OpenEntryStream().ToMaybe();
        }

        public IEnumerable<string> GetAllEntryNames()
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