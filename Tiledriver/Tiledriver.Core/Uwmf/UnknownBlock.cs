// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;

namespace Tiledriver.Core.Uwmf
{
    public sealed class UnknownBlock
    {
        public Identifier Name { get; }
        public List<UnknownProperty> Properties { get; } = new List<UnknownProperty>();

        public UnknownBlock(Identifier name)
        {
            Name = name;
        }
    }
}