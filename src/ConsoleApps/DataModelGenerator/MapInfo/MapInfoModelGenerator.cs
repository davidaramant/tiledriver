// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MapInfo;

public static class MapInfoModelGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		foreach (var block in MapInfoDefinitions.Blocks)
		{
			WriteRecord(basePath, block);
		}
	}

	static void WriteRecord(string basePath, IBlock block)
	{
		using var blockStream = File.CreateText(Path.Combine(basePath, block.ClassName + ".Generated.cs"));
		using var output = new IndentedWriter(blockStream);

		var containsCollection = block.OrderedProperties.Any(p => p is CollectionProperty);
		var containsIdentifier = block.OrderedProperties.Any(p => p is IdentifierProperty);

		List<string> includes = new() { "System.CodeDom.Compiler" };
		if (containsCollection)
		{
			includes.Add("System.Collections.Immutable");
		}

		if (containsIdentifier)
		{
			includes.Add("Tiledriver.Core.FormatModels.Common");
		}

		var qualifier = block is AbstractBlock ? "abstract" : "sealed";
		var includesNullables = block.OrderedProperties.OfType<ScalarProperty>().Any(sp => sp.IsNullable);

		if (includesNullables)
		{
			output.Line("#nullable enable");
		}

		output
			.WriteHeader("Tiledriver.Core.FormatModels.MapInfo", includes)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public {qualifier} partial record {block.ClassName}(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(GetPropertyDefinition))
			.DecreaseIndent();

		if (block is InheritedBlock ib)
		{
			output
				.Line($") : {ib.BaseClass.ClassName}(")
				.IncreaseIndent()
				.JoinLines(",", ib.BaseClass.OrderedProperties.Select(p => p.PropertyName))
				.DecreaseIndent();
		}

		output.Line(");");
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
