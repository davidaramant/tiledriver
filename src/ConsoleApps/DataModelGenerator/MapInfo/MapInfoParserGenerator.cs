// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using Humanizer;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MapInfo
{
    public static class MapInfoParserGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            using var stream = File.CreateText(Path.Combine(basePath, "MapDeclarationParser.Generated.cs"));
            using var output = new IndentedWriter(stream);

            var includes = new[]
            {
                "System.CodeDom.Compiler",
                "System.Linq",
                "Tiledriver.Core.FormatModels.Common.Reading",
                "Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree",
            };

            output
                .WriteHeader("Tiledriver.Core.FormatModels.MapInfo.Reading", includes)
                .OpenParen()
                .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
                .Line($"public static partial class MapDeclarationParser")
                .OpenParen();

            WriteParser(output, MapInfoDefinitions.Blocks.Single(b => b.ClassName == "DefaultMap"));
            output.Line();
            WriteParser(output, MapInfoDefinitions.Blocks.Single(b => b.ClassName == "AddDefaultMap"));

            output.CloseParen().CloseParen();
        }

        private static void WriteParser(IndentedWriter output, IBlock block)
        {
            output
                .Line($"private static partial {block.ClassName} Parse{block.ClassName}(ILookup<IdentifierToken, VariableAssignment> assignmentLookup)")
                .IncreaseIndent()
                .Line($"=> new {block.ClassName}(")
                .IncreaseIndent()
                .JoinLines(",", block.OrderedProperties.Select(GetPropertyReader))
                .DecreaseIndent()
                .Line(");")
                .DecreaseIndent();
        }

        private static string GetPropertyReader(Property property) =>
            $"{property.PropertyName}: " + property switch
            {
                FlagProperty => $"ReadFlag(assignmentLookup, \"{property.FormatName}\")",
                ScalarProperty sp => $"Read{sp.BasePropertyType.Pascalize()}Assignment(assignmentLookup, \"{property.FormatName}\")",
                CollectionProperty { PropertyName: "SpecialActions" } => "ReadSpecialActionAssignments(assignmentLookup)",
                CollectionProperty => $"ReadListAssignment(assignmentLookup, \"{property.FormatName}\")",
                _ => throw new Exception("What type of property is this??")
            };
    }
}