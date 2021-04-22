// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.UnifiedLexing;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading
{
    public static partial class UwmfSemanticAnalyzer
    {
        private static MapSquare ReadMapSquare(IntTuple tuple) =>
            tuple.Values.Count switch
            {
                3 => new MapSquare(
                    Tile: tuple.Values[0].Value,
                    Sector: tuple.Values[1].Value,
                    Zone: tuple.Values[2].Value),
                4 => new MapSquare(
                    Tile: tuple.Values[0].Value,
                    Sector: tuple.Values[1].Value,
                    Zone: tuple.Values[2].Value,
                    Tag: tuple.Values[3].Value),
                _ => throw new ParsingException($"Unexpected number of integers in MapSquare at {tuple.StartLocation} - expected 3 or 4.")
            };

        private static ImmutableArray<MapSquare> ReadPlaneMap(IntTupleBlock block)
        {
            // Since these things are immutable, there's no problem reusing references.
            var cache = new Dictionary<MapSquare, MapSquare>();
            var tileSpaces = new List<MapSquare>(block.Tuples.Length);

            foreach (var tuple in block.Tuples)
            {
                var ts = ReadMapSquare(tuple);

                if (!cache.ContainsKey(ts))
                {
                    cache.Add(ts, ts);
                }

                tileSpaces.Add(cache[ts]);
            }

            return tileSpaces.ToImmutableArray();
        }

        private static Token GetRequiredToken(
            IReadOnlyDictionary<Identifier, Token> fields,
            IdentifierToken contextName,
            Identifier fieldName)
        {
            if (!fields.ContainsKey(fieldName))
            {
                throw new ParsingException(
                    $"Missing required field {fieldName} in {contextName.Id} defined on {contextName.Location}");
            }

            return fields[fieldName];
        }

        private static T GetTokenValue<T>(Identifier fieldName, Token token) =>
            token is ValueToken<T> valueToken
                ? valueToken.Value
                : throw new ParsingException($"Expected {typeof(T).Name} for {fieldName} on {token.Location}");

        private static T GetRequiredFieldValue<T>(
            IReadOnlyDictionary<Identifier, Token> fields,
            IdentifierToken contextName,
            Identifier fieldName)
        {
            Token token = GetRequiredToken(fields, contextName, fieldName);
            return GetTokenValue<T>(fieldName, token);
        }

        private static T? GetOptionalFieldValue<T>(
            IReadOnlyDictionary<Identifier, Token> fields,
            Identifier fieldName,
            T defaultValue) =>
            fields.TryGetValue(fieldName, out var token)
                ? GetTokenValue<T>(fieldName, token)
                : defaultValue;

        private static double GetDoubleTokenValue(Identifier fieldName, Token token) =>
            token switch
            {
                IntegerToken it => it.Value,
                FloatToken ft => ft.Value,
                _ => throw new ParsingException($"Expected double for {fieldName} on {token.Location}")
            };

        private static double GetRequiredDoubleFieldValue(
            IReadOnlyDictionary<Identifier, Token> fields,
            IdentifierToken blockName,
            Identifier fieldName)
        {
            Token token = GetRequiredToken(fields, blockName, fieldName);
            return GetDoubleTokenValue(fieldName, token);
        }

        private static double? GetOptionalDoubleFieldValue(
            IReadOnlyDictionary<Identifier, Token> fields,
            Identifier fieldName) =>
            fields.TryGetValue(fieldName, out var token)
                ? GetDoubleTokenValue(fieldName, token)
                : default;
    }
}