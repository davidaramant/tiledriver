// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;

namespace Tiledriver.Core.Uwmf.Metadata
{
    public sealed class UwmfBlock
    {
        public Identifier Name { get; }
        public IEnumerable<UwmfProperty> Properties { get; }

        public UwmfBlock(string name, params UwmfProperty[] properties)
        {
            Name = new Identifier(name);
            Properties = properties;
        }
    }
}