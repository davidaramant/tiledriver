// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using Functional.Maybe;

namespace Tiledriver.Core.FormatModels
{
    public sealed class CompoundResourceProvider : IResourceProvider
    {
        private readonly Stack<IResourceProvider> _providers = new Stack<IResourceProvider>();

        public void AddProvider(IResourceProvider provider)
        {
            _providers.Push(provider);
        }

        public Stream Lookup(string path)
        {
            return TryLookup(path).OrElse(() => new EntryNotFoundException(path));
        }

        public Maybe<Stream> TryLookup(string path)
        {
            foreach (var provider in _providers)
            {
                var result = provider.TryLookup(path);
                if (result.IsSomething())
                {
                    return result;
                }
            }
            return Maybe<Stream>.Nothing;
        }

        public void Dispose()
        {
            foreach (var provider in _providers)
            {
                provider.Dispose();
            }
        }
    }
}