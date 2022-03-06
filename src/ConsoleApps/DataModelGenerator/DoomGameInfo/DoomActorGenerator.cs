// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tiledriver.DataModelGenerator.DoomGameInfo.Parsing;
using Tiledriver.DataModelGenerator.Utilities;
using Humanizer;

namespace Tiledriver.DataModelGenerator.DoomGameInfo;

internal static partial class DoomActorGenerator
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

        var categories = DoomActorParser.ParseCategories(reader);
        var actors = FlattenDefinitions(categories).ToList();

        WriteActorFile(basePath, actors);
    }

    static IEnumerable<Actor> FlattenDefinitions(IEnumerable<ActorCategory> categories) =>
        from category in categories
        from actor in category.Actors
        let lookup = new PropertyLookup(
            category.GlobalAssignments,
            actor.Assignments)
        select new Actor(
            Name: lookup.GetString("class"),
            CategoryName: category.Name.Humanize(LetterCasing.Title).Singularize(),
            Id: actor.Id,
            Description: lookup.GetString("title"),
            Radius: lookup.GetInt("width"), // This is named poorly in the config file
            Height: lookup.GetInt("height"));

    static void WriteActorFile(string basePath, IReadOnlyList<Actor> actors)
    {
        using var blockStream = File.CreateText(Path.Combine(basePath, "Actor.Generated.cs"));
        using var output = new IndentedWriter(blockStream);

        output
            .WriteHeader("Tiledriver.Core.GameInfo.Doom", new[] { "System.Collections.Generic", "System.CodeDom.Compiler" })
            .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
            .Line($"public sealed partial record Actor")
            .OpenParen();

        foreach (var actor in actors)
        {
            output
                .Line($"/// <summary>{actor.Description}</summary>")
                .Line($"public static readonly Actor {actor.SafeName} = new(")
                .IncreaseIndent()
                .JoinLines(",", new[] {
                    $"Id: {actor.Id}",
                    $"Description: \"{actor.Description}\"",
                    $"Width: {actor.Width}",
                    $"Height: {actor.Height}"
                })
                .DecreaseIndent()
                .Line(");")
                .Line();
        }

        foreach (var category in actors.GroupBy(c => c.CategoryName))
        {
            output
                .Line($"public static class {category.Key}")
                .OpenParen()
                .Lines(category.Select(actor=>$"public static Actor {actor.SafeName} => Actor.{actor.SafeName};"))
                .Line("public static IEnumerable<Actor> GetAll()")
                .OpenParen()
                .Lines(category.Select(actor=>$"yield return {actor.SafeName};"))
                .CloseParen()
                .CloseParen();
        }

        output
            .CloseParen();
    }

    sealed class PropertyLookup
    {
        private readonly string _context;
        private readonly IReadOnlyDictionary<string, object> _parentAssignments;
        private readonly IReadOnlyDictionary<string, object> _actorAssignments;

        public PropertyLookup(
            IReadOnlyDictionary<string, object> parentAssignments,
            IReadOnlyDictionary<string, object> actorAssignments)
        {
            _parentAssignments = parentAssignments;
            _actorAssignments = actorAssignments;
            _context = GetString("class");
        }

        private object GetValue(string name) =>
            _actorAssignments.TryLookupValue(name) ??
            _parentAssignments.TryLookupValue(name) ??
            throw new ArgumentException($"Could not find {name} for {_context}");

        public int GetInt(string name) => (int)GetValue(name);
        public string GetString(string name) => (string)GetValue(name);
    }

}
