// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace Tiledriver.DataModelGenerator.DoomGameInfo;

internal static class DoomActorGenerator
{
    public static void WriteToPath(string basePath)
    {
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Tiledriver.DataModelGenerator.DoomGameInfo.DoomActors.txt";

        using Stream stream = assembly.GetManifestResourceStream(resourceName) ?? throw new Exception($"Could not find {resourceName}");
        using StreamReader reader = new(stream);

        var categories = ParseCategories(reader);
    }

    interface IAssignment { string Name { get; } }
    sealed record StringAssignment(string Name, string Value) : IAssignment;
    sealed record IntegerAssignment(string Name, int Value) : IAssignment;
    sealed record ActorDefinition(int Id, IReadOnlyList<IAssignment> Assignments);
    sealed record ActorCategory(string Name, IReadOnlyList<IAssignment> GlobalAssignments, IReadOnlyList<ActorDefinition> Actors);

    static IEnumerable<ActorCategory> ParseCategories(StreamReader reader)
    {
        throw new NotImplementedException();
    }

    static class DoomActorParser
    {
        static readonly Parser<char, char> LBrace = Char('{');
        static readonly Parser<char, char> RBrace = Char('}');
        static readonly Parser<char, char> Quote = Char('"');
        static readonly Parser<char, char> Equal = Char('=');
        static readonly Parser<char, char> EqualWhitespace = Equal.Between(SkipWhitespaces);
        static readonly Parser<char, char> SemiColon = Char(';');
        static readonly Parser<char, char> SemiColonWhitespace = SemiColon.Between(SkipWhitespaces);

        private static readonly Parser<char, string> String =
            Token(c => c != '"')
            .ManyString()
            .Between(Quote);
        private static readonly Parser<char, int> Integer =
            Token(char.IsDigit).ManyString().Select(int.Parse);

        static readonly Parser<char, string> Identifier = Token(char.IsLower).ManyString();

        static readonly Parser<char, IAssignment> StringAssignment =
            from id in Identifier
            from eq in EqualWhitespace
            from value in String
            from sc in SemiColonWhitespace
            select (IAssignment)(new StringAssignment(id, value));

        static readonly Parser<char, IAssignment> IntegerAssignment =
            from id in Identifier
            from eq in EqualWhitespace
            from value in Integer
            from sc in SemiColonWhitespace
            select (IAssignment)(new IntegerAssignment(id, value));

        

        //private static readonly Parser<char, string> String =
        //Token(c => c != '"')
        //    .ManyString()
        //    .Between(Quote);
        //private static readonly Parser<char, IJson> JsonString =
        //    String.Select<IJson>(s => new JsonString(s));

        //private static readonly Parser<char, IJson> Json =
        //    JsonString.Or(Rec(() => JsonArray!)).Or(Rec(() => JsonObject!));

        //private static readonly Parser<char, IJson> JsonArray =
        //    Json.Between(SkipWhitespaces)
        //        .Separated(Comma)
        //        .Between(LBracket, RBracket)
        //        .Select<IJson>(els => new JsonArray(els.ToImmutableArray()));

        //private static readonly Parser<char, KeyValuePair<string, IJson>> JsonMember =
        //    String
        //        .Before(ColonWhitespace)
        //        .Then(Json, (name, val) => new KeyValuePair<string, IJson>(name, val));

        //private static readonly Parser<char, IJson> JsonObject =
        //    JsonMember.Between(SkipWhitespaces)
        //        .Separated(Comma)
        //        .Between(LBrace, RBrace)
        //        .Select<IJson>(kvps => new JsonObject(kvps.ToImmutableDictionary()));

        //public static Result<char, IJson> Parse(string input) => Json.Parse(input);
    }
}
