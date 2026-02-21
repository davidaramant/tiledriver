using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Uwmf;

public static class UwmfModelGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		foreach (var block in UwmfDefinitions.Blocks)
		{
			WriteRecord(basePath, block);
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
			.WriteHeader("Tiledriver.Core.FormatModels.Uwmf", includes)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public sealed partial record {block.ClassName}(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetPropertyDefinition))
			.DecreaseIndent()
			.Line(");");
	}

	static string GetPropertyDefinition(Property property)
	{
		var definition = $"{property.PropertyType} {property.PropertyName}";

		var defaultString = property.DefaultString;
		if (defaultString != null)
		{
			definition += $" = {defaultString}";
		}

		return definition;
	}
}
