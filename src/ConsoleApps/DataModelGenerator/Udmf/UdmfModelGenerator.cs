// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Udmf;

public static class UdmfModelGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		foreach (var block in UdmfDefinitions.Blocks)
		{
			WriteRecord(basePath, block);
			if (block.Serialization != SerializationType.TopLevel)
			{
				WriteBuilder(basePath, block);
			}
		}
	}

	static void WriteRecord(string basePath, Block block)
	{
		using var blockStream = File.CreateText(Path.Combine(basePath, block.ClassName + ".Generated.cs"));
		using var output = new IndentedWriter(blockStream);

		var containsTexture = block.Properties.Any(p => p is TextureProperty);
		var containsCollection = block.Properties.Any(p => p is CollectionProperty);

		List<string> includes = ["System.CodeDom.Compiler"];
		if (containsTexture)
		{
			includes.Add("Tiledriver.Core.FormatModels.Common");
		}
		if (containsCollection)
		{
			includes.Add("System.Collections.Immutable");
		}

		output
			.WriteHeader("Tiledriver.Core.FormatModels.Udmf", includes)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public sealed partial record {block.ClassName}(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetRecordPropertyDefinition))
			.DecreaseIndent()
			.Line(");");
	}

	static void WriteBuilder(string basePath, Block block)
	{
		using var blockStream = File.CreateText(Path.Combine(basePath, block.ClassName + "Builder.Generated.cs"));
		using var output = new IndentedWriter(blockStream);

		var containsTexture = block.Properties.Any(p => p is TextureProperty);
		var containsNullables = block.Properties.Any(p => !p.HasDefault);
		List<string> includes = ["System.CodeDom.Compiler", "System"];
		if (containsTexture)
		{
			includes.Add("Tiledriver.Core.FormatModels.Common");
		}

		output
			.WriteHeader("Tiledriver.Core.FormatModels.Udmf", includes, enableNullables: containsNullables)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public sealed partial class {block.ClassName}Builder")
			.OpenParen()
			.JoinLines("", block.OrderedProperties.Select(GetBuilderPropertyDefinition))
			.Line()
			.Line($"public {block.ClassName} Build() =>")
			.IncreaseIndent()
			.Line("new(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetBuilderPropertyAssignment))
			.DecreaseIndent()
			.Line(");")
			.DecreaseIndent()
			.CloseParen();
	}

	static string GetRecordPropertyDefinition(Property property)
	{
		var definition = $"{property.PropertyType} {property.PropertyName}";

		var defaultString = property.DefaultString;
		if (defaultString != null)
		{
			definition += $" = {defaultString}";
		}

		return definition;
	}

	static string GetBuilderPropertyDefinition(Property property)
	{
		var type = property.PropertyType + (property.HasDefault ? "" : "?");

		var definition = $"{type} {property.PropertyName} {{ get; set; }}";

		var defaultString = property.DefaultString;
		if (defaultString != null)
		{
			definition += $" = {defaultString};";
		}

		return definition;
	}

	static string GetBuilderPropertyAssignment(Property property)
	{
		var normalAssignment = $"{property.PropertyName}: {property.PropertyName}";

		if (!property.HasDefault)
		{
			if (property is TextureProperty { IsOptional: true })
			{
				normalAssignment += " ?? Texture.None";
			}
			else
			{
				normalAssignment +=
					$" ?? throw new ArgumentNullException(\"{property.PropertyName} must have a value assigned.\")";
			}
		}

		return normalAssignment;
	}
}
