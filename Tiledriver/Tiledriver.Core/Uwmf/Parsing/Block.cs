// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tiledriver.Core.Uwmf.Parsing
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class Block : IEnumerable<Assignment>
    {
        private readonly List<Assignment> _properties;
        public Identifier Name { get; }

        public Block(Identifier name, IEnumerable<Assignment> propertyAssignments)
        {
            Name = name;
            _properties = propertyAssignments.ToList();

            if (_properties.Select(p => p.Name).Distinct().Count() != _properties.Count)
            {
                throw new ArgumentException($"Duplicate property assignments found in {Name}", nameof(propertyAssignments));
            }
        }

        public IEnumerator<Assignment> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString() => Name + "\n" + string.Join("\n", _properties.Select(u => $"* {u}"));
    }
}