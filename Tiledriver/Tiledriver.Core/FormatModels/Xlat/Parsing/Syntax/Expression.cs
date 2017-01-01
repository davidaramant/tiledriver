// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class Expression
    {
        private readonly Dictionary<Identifier, Token> _properties;
        
        public Expression(
            Maybe<Identifier> name,
            Maybe<ushort> oldnum,
            IEnumerable<Identifier> qualifiers,
            IEnumerable<KeyValuePair<Identifier, Token>> properties,
            IEnumerable<Token> values,
            IEnumerable<Expression> subExpressions )
        {
            Name = name;
            Oldnum = oldnum;
            Qualifiers = qualifiers;
            _properties = properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Values = values;
            SubExpressions = subExpressions;
        }

        public static Expression Simple(
            Identifier name,
            Maybe<ushort> oldnum,
            IEnumerable<Identifier> qualifiers)
        {
            return new Expression(
                name: name.ToMaybe(),
                oldnum: oldnum, 
                qualifiers: qualifiers,
                properties: Enumerable.Empty<KeyValuePair<Identifier, Token>>(),
                values: Enumerable.Empty<Token>(),
                subExpressions: Enumerable.Empty<Expression>());
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
                values: Enumerable.Empty<Token>(),
                subExpressions:Enumerable.Empty<Expression>());
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
                values: Enumerable.Empty<Token>(),
                subExpressions: Enumerable.Empty<Expression>());
        }

        public static Expression Block(
            Identifier name,
            IEnumerable<Expression> subExpressions)
        {
            return new Expression(
                name: name.ToMaybe(),
                oldnum: Maybe<ushort>.Nothing, 
                qualifiers: Enumerable.Empty<Identifier>(),
                properties: Enumerable.Empty<KeyValuePair<Identifier, Token>>(),
                values: Enumerable.Empty<Token>(),
                subExpressions: subExpressions);
        }

        public IEnumerable<Assignment> GetAssignments()
        {
            return _properties.Select(pair => new Assignment(pair.Key, pair.Value));
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
        public IEnumerable<Identifier> Qualifiers { get; }
        public IEnumerable<Token> Values { get; }
        public IEnumerable<Expression> SubExpressions { get; }
    }
}