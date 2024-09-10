// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Uwmf;

public static class UwmfWriterGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		using var stream = File.CreateText(Path.Combine(basePath, "UwmfWriter.Generated.cs"));
		using var output = new IndentedWriter(stream);

		output
			.WriteHeader("Tiledriver.Core.FormatModels.Uwmf.Writing", new[] { "System.CodeDom.Compiler", "System.IO" })
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public static partial class UwmfWriter")
			.OpenParen();

		foreach (var block in UwmfDefinitions.Blocks.Where(b => b.Serialization != SerializationType.Custom))
		{
			CreateBlockWriter(output, block);
		}

		output.CloseParen();
	}

	private static void CreateBlockWriter(IndentedWriter output, Block block)
	{
		output.Line($"private static void Write(StreamWriter writer, {block.ClassName} {block.Name})").OpenParen();

		if (block.Serialization == SerializationType.Normal)
		{
			output.Line($"writer.WriteLine(\"{block.Name}\");").Line("writer.WriteLine(\"{\");");
		}

		foreach (var p in block.Properties)
		{
			if (p is ScalarProperty sp)
			{
				var defaultValue = sp.DefaultString;

				string optionalDefault = defaultValue != null ? ", " + defaultValue : string.Empty;
				string indent = block.Serialization == SerializationType.Normal ? string.Empty : ", indent:false";

				output.Line(
					$"WriteProperty(writer, \"{sp.FormatName}\", {block.Name}.{sp.PropertyName}{optionalDefault}{indent});"
				);
			}
			else if (p is CollectionProperty cp)
			{
				output
					.Line($"foreach(var block in {block.Name}.{cp.PropertyName})")
					.OpenParen()
					.Line($"Write(writer, block);")
					.CloseParen();
			}
		}

		if (block.Serialization == SerializationType.Normal)
		{
			output.Line("writer.WriteLine(\"}\");");
		}
		output.CloseParen();
	}
}
