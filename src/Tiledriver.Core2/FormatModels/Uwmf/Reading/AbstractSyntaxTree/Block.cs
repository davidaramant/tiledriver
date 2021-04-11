// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Common;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree
{
    public sealed record Block(IdentifierToken Name, ImmutableArray<Assignment> Fields) : IGlobalExpression
    {
        public IReadOnlyDictionary<Identifier, Token> GetFieldAssignments()
        {
            var assignments = new Dictionary<Identifier, Token>();

            foreach (var field in Fields)
            {
                if (assignments.ContainsKey(field.Name.Id))
                {
                    throw new ParsingException($"Duplicate field definition found: {field.Name.Id} on {field.Name.Location}");
                }

                assignments.Add(field.Name.Id, field.Value);
            }

            return assignments;
        }
    }
}