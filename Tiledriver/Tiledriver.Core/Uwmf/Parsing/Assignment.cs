// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Diagnostics;

namespace Tiledriver.Core.Uwmf.Parsing
{
    [DebuggerDisplay("{Name} = {Value}")]
    public sealed class Assignment
    {
        public Identifier Name { get; }
        public Token Value { get; }

        public Assignment(Identifier name, Token value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"{Name} = {Value}";
    }
}