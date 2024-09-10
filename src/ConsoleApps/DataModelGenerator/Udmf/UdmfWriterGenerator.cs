// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Udmf;

public static class UdmfWriterGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		using var stream = File.CreateText(Path.Combine(basePath, "UdmfWriter.Generated.cs"));
		using var output = new IndentedWriter(stream);

		output
			.WriteHeader(
				"Tiledriver.Core.FormatModels.Udmf.Writing",
				["System.CodeDom.Compiler", "System.IO", "Tiledriver.Core.FormatModels.Common"]
			)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line($"public static partial class UdmfWriter")
			.OpenParen();

		foreach (var block in UdmfDefinitions.Blocks.Where(b => b.Serialization != SerializationType.Custom))
		{
			CreateBlockWriter(output, block);
		}

		output.CloseParen();
	}

	private static void CreateBlockWriter(IndentedWriter output, Block block)
	{
		output
			.Line($"private static void Write(StreamWriter writer, {block.ClassName} {block.Name}, int index = 0)")
			.OpenParen();

		if (block.Serialization == SerializationType.Normal)
		{
			output.Line($"writer.WriteLine($\"{block.Name} // {{index}}\");").Line("writer.WriteLine(\"{\");");
		}

		foreach (var p in block.Properties)
		{
			if (p is ScalarProperty sp)
			{
				var defaultValue = sp.DefaultString;
				if (sp is TextureProperty { IsOptional: true })
				{
					defaultValue = "Texture.None";
				}

				string optionalDefault = defaultValue != null ? ", " + defaultValue : string.Empty;

				output.Line(
					$"WriteProperty(writer, \"{sp.FormatName}\", {block.Name}.{sp.PropertyName}{optionalDefault});"
				);
			}
			else if (p is CollectionProperty cp)
			{
				output
					.Line($"for(int i = 0; i < {block.Name}.{cp.PropertyName}.Length; i++)")
					.OpenParen()
					.Line($"Write(writer, {block.Name}.{cp.PropertyName}[i], i);")
					.CloseParen();
			}
		}

		if (block.Serialization == SerializationType.Normal)
		{
			output.Line("writer.WriteLine(\"}\");").Line("writer.WriteLine();");
		}
		output.CloseParen();
	}
}
