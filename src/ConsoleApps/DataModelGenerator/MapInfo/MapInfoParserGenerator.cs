// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Humanizer;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MapInfo;

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
			"Tiledriver.Core.FormatModels.Common",
			"Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree",
		};

		output
			.Line("#nullable enable")
			.WriteHeader("Tiledriver.Core.FormatModels.MapInfo.Reading", includes)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public static partial class MapDeclarationParser")
			.OpenParen();

		WriteParser(output, MapInfoDefinitions.Blocks.Single(b => b.ClassName == "DefaultMap"));
		output.Line();
		WriteParser(output, MapInfoDefinitions.Blocks.Single(b => b.ClassName == "AddDefaultMap"));
		output.Line();
		WriteMapParser(output, MapInfoDefinitions.Blocks.Single(b => b.ClassName == "Map"));
		output.Line();
		WriteDefaultMapUpdater(output, MapInfoDefinitions.Blocks.Single(b => b.ClassName == "DefaultMap"));

		output.CloseParen();
	}

	private static void WriteParser(IndentedWriter output, IBlock block)
	{
		output
			.Line(
				$"private static partial {block.ClassName} Parse{block.ClassName}(ILookup<Identifier, VariableAssignment> assignmentLookup)"
			)
			.IncreaseIndent()
			.Line($"=> new {block.ClassName}(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetPropertyReader))
			.DecreaseIndent()
			.Line(");")
			.DecreaseIndent();
	}

	private static void WriteMapParser(IndentedWriter output, IBlock block)
	{
		output
			.Line("private static partial Map ParseMap(")
			.IncreaseIndent()
			.Line("ILookup<Identifier, VariableAssignment> assignmentLookup,")
			.Line("string mapLump,")
			.Line("string? mapName,")
			.Line("bool isMapNameLookup,")
			.Line("DefaultMap defaultMap) =>")
			.Line("new Map(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetMapPropertyReader))
			.DecreaseIndent()
			.Line(");")
			.DecreaseIndent();
	}

	private static string GetMapPropertyReader(Property property) =>
		$"{property.PropertyName}: "
		+ property switch
		{
			ScalarProperty { PropertyName: "MapName" } => "mapName",
			ScalarProperty { PropertyName: "MapLump" } => "mapLump",
			ScalarProperty { PropertyName: "IsMapNameLookup" } => "isMapNameLookup",
			FlagProperty =>
				$"ReadFlag(assignmentLookup, \"{property.FormatName}\") ?? defaultMap.{property.PropertyName}",
			ScalarProperty sp =>
				$"Read{sp.BasePropertyType.Pascalize()}Assignment(assignmentLookup, \"{property.FormatName}\") ?? defaultMap.{property.PropertyName}",
			CollectionProperty { PropertyName: "SpecialActions" } =>
				"ReadSpecialActionAssignments(assignmentLookup).AddRange(defaultMap.SpecialActions)",
			CollectionProperty =>
				$"ReadListAssignment(assignmentLookup, \"{property.FormatName}\").AddRange(defaultMap.{property.PropertyName})",
			_ => throw new Exception("What type of property is this??"),
		};

	private static string GetPropertyReader(Property property) =>
		$"{property.PropertyName}: "
		+ property switch
		{
			FlagProperty => $"ReadFlag(assignmentLookup, \"{property.FormatName}\")",
			ScalarProperty sp =>
				$"Read{sp.BasePropertyType.Pascalize()}Assignment(assignmentLookup, \"{property.FormatName}\")",
			CollectionProperty { PropertyName: "SpecialActions" } => "ReadSpecialActionAssignments(assignmentLookup)",
			CollectionProperty => $"ReadListAssignment(assignmentLookup, \"{property.FormatName}\")",
			_ => throw new Exception("What type of property is this??"),
		};

	private static void WriteDefaultMapUpdater(IndentedWriter output, IBlock block)
	{
		output
			.Line(
				$"private static partial DefaultMap UpdateDefaultMap(DefaultMap defaultMap, AddDefaultMap addDefaultMap) =>"
			)
			.IncreaseIndent()
			.Line("new DefaultMap(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetPropertyCondenser))
			.DecreaseIndent()
			.Line(");")
			.DecreaseIndent();
	}

	private static string GetPropertyCondenser(Property property) =>
		$"{property.PropertyName}: "
		+ property switch
		{
			ScalarProperty sp => $"addDefaultMap.{sp.PropertyName} ?? defaultMap.{sp.PropertyName}",
			CollectionProperty cp => $"defaultMap.{cp.PropertyName}.AddRange(addDefaultMap.{cp.PropertyName})",
			_ => throw new Exception("What type of property is this??"),
		};
}
