// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

namespace Tiledriver.Core.Uwmf
{
    public sealed class UnknownProperty
    {
        public Identifier Name { get; }
        public string Value { get; }

        public UnknownProperty(Identifier name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}