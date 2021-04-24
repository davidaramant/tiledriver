// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using System.Collections.Generic;

namespace Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree
{
    public static class FieldAssignmentExtensions
    {
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

        public static T GetRequiredFieldValue<T>(
            this IReadOnlyDictionary<Identifier, Token> fields,
            IdentifierToken contextName,
            Identifier fieldName)
        {
            Token token = GetRequiredToken(fields, contextName, fieldName);
            return GetTokenValue<T>(fieldName, token);
        }

        public static T? GetOptionalFieldValue<T>(
            this IReadOnlyDictionary<Identifier, Token> fields,
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

        public static double GetRequiredDoubleFieldValue(
            this IReadOnlyDictionary<Identifier, Token> fields,
            IdentifierToken blockName,
            Identifier fieldName)
        {
            Token token = GetRequiredToken(fields, blockName, fieldName);
            return GetDoubleTokenValue(fieldName, token);
        }
    }
}