// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Xlat;

public static class XlatParserGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		using var stream = File.CreateText(Path.Combine(basePath, "XlatParser.Generated.cs"));
		using var output = new IndentedWriter(stream);

		var includes = new[]
		{
			"System.CodeDom.Compiler",
			"Tiledriver.Core.FormatModels.Common.Reading",
			"Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree",
		};

		output
			.WriteHeader("Tiledriver.Core.FormatModels.Xlat.Reading", includes)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public static partial class XlatParser")
			.OpenParen();

		foreach (var block in XlatDefinitions.Blocks.Where(b => b.Serialization == SerializationType.Normal))
		{
			CreateBlockReader(output, block);
		}

		output.CloseParen();
	}

	private static string CreateParameterAssignment(Property property, string context = "block.Name") =>
		property switch
		{
			ScalarProperty sp => CreateParameterAssignment(sp, context),
			CollectionProperty cp => CreateParameterAssignment(cp),
			_ => throw new Exception("Unknown property type"),
		};

	private static string CreateParameterAssignment(ScalarProperty property, string context = "block.Name")
	{
		var getValue =
			property.DefaultString == null
				? $"fields.GetRequiredFieldValue<{property.PropertyType}>({context}, \"{property.FormatName}\")"
				: $"fields.GetOptionalFieldValue<{property.PropertyType}>(\"{property.FormatName}\", {property.DefaultString})";

		return $"{property.PropertyName}: {getValue}";
	}

	private static string CreateParameterAssignment(CollectionProperty property)
	{
		return $"{property.PropertyName}: {property.Name}Builder.ToImmutable()";
	}

	private static void CreateBlockReader(IndentedWriter output, Block block)
	{
		output
			.Line($"private static {block.ClassName} Read{block.ClassName}(ushort oldNum, Block block)")
			.OpenParen()
			.Line("var fields = block.GetFieldAssignments();")
			.Line()
			.Line($"return new {block.ClassName}(")
			.IncreaseIndent()
			.Line("oldNum,")
			.JoinLines(
				",",
				block.OrderedProperties.Where(p => p.Name != "oldNum").Select(p => CreateParameterAssignment(p))
			)
			.DecreaseIndent()
			.Line(");")
			.CloseParen();
	}
}
