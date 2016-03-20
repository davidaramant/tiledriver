// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.Uwmf.Metadata
{
    public sealed class UwmfBlock
    {
        private readonly List<Identifier> _subBlocks = new List<Identifier>();

        public Identifier Name { get; }
        public IEnumerable<UwmfProperty> Properties { get; }
        public IEnumerable<Identifier> SubBlocks => _subBlocks;
        public bool IsSubBlock { get; private set; } = true;
        public bool NormalWriting { get; private set; } = true;

        public UwmfBlock(string name, params UwmfProperty[] properties)
        {
            Name = new Identifier(name);
            Properties = properties;
        }

        public UwmfBlock HasSubBlocks(params string[] names)
        {
            _subBlocks.AddRange(names.Select(_ => new Identifier(_)));
            return this;
        }

        public UwmfBlock IsTopLevel()
        {
            IsSubBlock = false;
            return this;
        }

        public UwmfBlock DisableNormalWriting()
        {
            NormalWriting = false;
            return this;
        }
    }
}