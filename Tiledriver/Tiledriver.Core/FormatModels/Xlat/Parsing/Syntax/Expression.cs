// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class Expression : IExpression
    {
        private readonly Dictionary<Identifier, Token> _properties;
        private readonly IEnumerable<Identifier> _qualifiers;
        private readonly IEnumerable<Token> _values;

        public Expression(
            Maybe<Identifier> name,
            Maybe<ushort> oldnum,
            IEnumerable<Identifier> qualifiers,
            IEnumerable<KeyValuePair<Identifier, Token>> properties,
            IEnumerable<Token> values)
        {
            Name = name;
            Oldnum = oldnum;
            _qualifiers = qualifiers;
            _properties = properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            _values = values;
        }

        public static Expression PropertyList(
            Maybe<Identifier> name,
            Maybe<ushort> oldnum,
            IEnumerable<Identifier> qualifiers,
            IEnumerable<KeyValuePair<Identifier, Token>> properties)
        {
            return new Expression(
                name: name,
                oldnum: oldnum,
                qualifiers: qualifiers,
                properties: properties,
                values: Enumerable.Empty<Token>());
        }

        public static Expression ValueList(
            Maybe<Identifier> name,
            Maybe<ushort> oldnum,
            IEnumerable<Identifier> qualifiers,
            IEnumerable<Token> values)
        {
            return new Expression(
                name: name,
                oldnum: oldnum,
                qualifiers: qualifiers,
                properties: Enumerable.Empty<KeyValuePair<Identifier, Token>>(),
                values: Enumerable.Empty<Token>());
        }

        public IEnumerator<Assignment> GetEnumerator()
        {
            return _properties.Select(pair => new Assignment(pair.Key, pair.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasAssignments => _properties.Any();

        public Maybe<Token> GetValueFor(string name)
        {
            return GetValueFor(new Identifier(name));
        }

        public Maybe<Token> GetValueFor(Identifier name)
        {
            return _properties.Lookup(name);
        }

        public Maybe<Identifier> Name { get; }
        public Maybe<ushort> Oldnum { get; }
        public bool HasQualifiers => _qualifiers.Any();
        public IEnumerable<Identifier> Qualifiers => _qualifiers;
        public bool HasValues => _values.Any();
        public IEnumerable<Token> Values => _values;
    }
}