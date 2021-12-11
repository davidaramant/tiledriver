// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tiledriver.DataModelGenerator.DoomGameInfo.Parsing;
using Tiledriver.DataModelGenerator.Utilities;

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
        var definitions = FlattenDefinitions(categories);

        WriteActorFile(basePath, definitions);
    }

    static IEnumerable<Actor> FlattenDefinitions(IEnumerable<ActorCategory> categories)
    {
        foreach (var category in categories)
        {
            foreach (var actor in category.Actors)
            {
                yield return new Actor(
                    Name: actor.Assignments["class"].ToString() ?? throw new Exception("No class in actor"),
                    Id: actor.Id,
                    Description: actor.Assignments["title"].ToString() ?? throw new Exception("No title in actor"));
            }
        }
    }

    static void WriteActorFile(string basePath, IEnumerable<Actor> actors)
    {
        using var blockStream =
               File.CreateText(Path.Combine(basePath, "Actor.Generated.cs"));
        using var output = new IndentedWriter(blockStream);

        output
            .WriteHeader("Tiledriver.Core.GameInfo.Doom", new[] { "System.Collections.Generic", "System.CodeDom.Compiler" })
            .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
            .Line($"public sealed partial record Actor")
            .OpenParen();

        foreach(var actor in actors)
        {

            output
                .Line($"/// <summary>{actor.Description}</summary>")
                .Line($"public static readonly Actor {actor.SafeName} = new({actor.Id}, \"{actor.Description}\");")
                .Line();
        }

        output
            .CloseParen();
    }

}
