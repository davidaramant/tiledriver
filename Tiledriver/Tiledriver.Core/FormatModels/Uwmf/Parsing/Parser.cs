// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Uwmf.Parsing.Syntax;

namespace Tiledriver.Core.FormatModels.Uwmf.Parsing
{
    public static partial class Parser
    {
        public static Map Parse(UwmfSyntaxTree syntaxTree)
        {
            var map = new Map();

            SetGlobalAssignments(map, syntaxTree);
            SetBlocks(map, syntaxTree);
            map.PlaneMaps.AddRange(syntaxTree.ArrayBlocks.Select(ParsePlaneMap));

            map.CheckSemanticValidity();

            return map;
        }

        static partial void SetGlobalAssignments(Map map, UwmfSyntaxTree tree);
        static partial void SetBlocks(Map map, UwmfSyntaxTree tree);

        private static PlaneMap ParsePlaneMap(ArrayBlock arrayBlock)
        {
            return new PlaneMap(
                arrayBlock.Select(tuple =>
                {
                    if (tuple.Count < 3 || tuple.Count > 4)
                    {
                        throw new UwmfParsingException("Invalid number of entries inside a tilespace.");
                    }

                    return new TileSpace(
                        tile: tuple[0],
                        sector: tuple[1],
                        zone: tuple[2],
                        tag: tuple.ElementAtOrDefault(3));
                }));
        }

        private static void SetRequiredString(Maybe<Token> maybeToken, Action<string> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new UwmfParsingException($"{parameterName} was not set on {blockName}")).
                TryAsString().
                OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not a string.")));
        }

        private static void SetRequiredFloatingPointNumber(Maybe<Token> maybeToken, Action<double> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new UwmfParsingException($"{parameterName} was not set on {blockName}")).
                TryAsDouble().
                OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not a floating point value.")));
        }

        private static void SetRequiredIntegerNumber(Maybe<Token> maybeToken, Action<int> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new UwmfParsingException($"{parameterName} was not set on {blockName}")).
                TryAsInt().
                OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not an integer.")));
        }

        private static void SetRequiredBoolean(Maybe<Token> maybeToken, Action<bool> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new UwmfParsingException($"{parameterName} was not set on {blockName}")).
                TryAsBool().
                OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not a boolean.")));
        }

        private static void SetOptionalString(Maybe<Token> maybeToken, Action<string> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsString().
                    OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not a string.")))
                .Do(setter);
        }

        private static void SetOptionalFloatingPointNumber(Maybe<Token> maybeToken, Action<double> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsDouble().
                    OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not a floating point value.")))
                .Do(setter);
        }

        private static void SetOptionalIntegerNumber(Maybe<Token> maybeToken, Action<int> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsInt().
                    OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not an integer.")))
                .Do(setter);
        }

        private static void SetOptionalBoolean(Maybe<Token> maybeToken, Action<bool> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsBool().
                    OrElse(() => new UwmfParsingException($"{parameterName} in {blockName} was not a boolean.")))
                .Do(setter);
        }
    }
}