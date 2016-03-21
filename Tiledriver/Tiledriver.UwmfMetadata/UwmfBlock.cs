// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.UwmfMetadata
{
    public sealed class UwmfBlock : NamedItem
    {
        private readonly List<NamedItem> _subBlocks = new List<NamedItem>();

        public IEnumerable<UwmfProperty> Properties { get; }
        public IEnumerable<NamedItem> SubBlocks => _subBlocks;
        public bool IsSubBlock { get; private set; } = true;
        public bool NormalWriting { get; private set; } = true;

        public UwmfBlock(string name, params UwmfProperty[] properties) : base(name)
        {
            Properties = properties;
        }

        public UwmfBlock HasSubBlocks(params string[] names)
        {
            _subBlocks.AddRange(names.Select(_ => new NamedItem(_)));
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