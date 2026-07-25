using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Udmf;

public static class UdmfParserGenerator
{
	public static void WriteToPath(string basePath)
	{
		if (!Directory.Exists(basePath))
		{
			Directory.CreateDirectory(basePath);
		}

		using var stream = File.CreateText(Path.Combine(basePath, "UdmfParser.Generated.cs"));
		using var output = new IndentedWriter(stream);

		var includes = new[]
		{
			"System.CodeDom.Compiler",
			"System.Collections.Generic",
			"System.Collections.Immutable",
			"System.Runtime.CompilerServices",
			"Tiledriver.FormatModels.Common",
			"Tiledriver.FormatModels.Udmf",
		};

		output
			.WriteHeader("Tiledriver.FormatModels.Udmf.Reading", includes, enableNullables: true)
			.Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
			.Line("public sealed partial class UdmfParser")
			.OpenParen();

		foreach (var block in UdmfDefinitions.Blocks.Where(b => b.Serialization == SerializationType.Normal))
		{
			CreateSeenFieldsEnum(output, block);
			CreateBlockParser(output, block);
		}

		CreateBlockDispatcher(
			output,
			UdmfDefinitions.Blocks.Single(b => b.Serialization == SerializationType.TopLevel)
		);

		CreateMapDataFactory(output, UdmfDefinitions.Blocks.Single(b => b.Serialization == SerializationType.TopLevel));

		output.CloseParen();
	}

	private static string CreateCollectionBuilderFieldName(CollectionProperty property) => $"_{property.Name}Builder";

	private static string CreateModelAssignment(Property property) =>
		property is CollectionProperty collectionProperty
			? $"{property.PropertyName}: {CreateCollectionBuilderFieldName(collectionProperty)}.ToImmutable()"
			: $"{property.PropertyName}: {property.Name}";

	private static string CreateTopLevelAssignment(Property property) =>
		property switch
		{
			CollectionProperty collectionProperty => CreateModelAssignment(collectionProperty),
			StringProperty stringProperty => stringProperty.FormatName == "namespace"
				? $"{property.PropertyName}: _namespace ?? throw new ParsingException(\"Missing required field 'namespace'\")"
				: $"{property.PropertyName}: _{property.Name}",
			_ => throw new Exception($"Unsupported top-level property type: {property.GetType().Name}"),
		};

	private static void CreateSeenFieldsEnum(IndentedWriter output, Block block)
	{
		var scalarProperties = block.Properties.OfType<ScalarProperty>().ToArray();
		ValidateSeenFieldCount(block, scalarProperties.Length);

		output
			.Line("[global::System.Flags]")
			.Line($"private enum {CreateSeenFieldsEnumName(block)} : uint")
			.OpenParen()
			.Line("None = 0,")
			.Lines(
				scalarProperties.Select(
					(property, index) => $"{CreateSeenFieldName(property)} = {CreateSeenMaskLiteral(index)},"
				)
			)
			.CloseParen()
			.Line("[MethodImpl(MethodImplOptions.AggressiveInlining)]")
			.Line(
				$"private static bool HasFlag({CreateSeenFieldsEnumName(block)} value, {CreateSeenFieldsEnumName(block)} flag) => (value & flag) == flag;"
			)
			.Line();
	}

	private static void CreateBlockParser(IndentedWriter output, Block block)
	{
		var scalarProperties = block.Properties.OfType<ScalarProperty>().ToArray();
		string seenFieldsEnumName = CreateSeenFieldsEnumName(block);

		output
			.Line($"private {block.ClassName} Parse{block.ClassName}Block(Identifier blockName)")
			.OpenParen()
			.Lines(scalarProperties.Select(CreateLocalDeclaration))
			.Line($"{seenFieldsEnumName} seenFields = {seenFieldsEnumName}.None;")
			.Line("HashSet<Identifier>? unknownFields = null;")
			.Line()
			.Line("while (!_lexer.TryExpectCloseBrace())")
			.OpenParen()
			.Line("Identifier identifier = _lexer.ReadIdentifier();")
			.Line("string identifierText = (string)identifier;")
			.Line("_lexer.ExpectEquals();")
			.Line("bool handledKnownField = false;")
			.Line("switch (identifierText.Length)")
			.OpenParen();

		CreateFieldParseBranches(output, block);

		output
			.Line("default:")
			.IncreaseIndent()
			.Line("handledKnownField = false;")
			.Line("break;")
			.DecreaseIndent()
			.CloseParen()
			.Line("if (!handledKnownField)")
			.OpenParen()
			.Line("unknownFields ??= [];")
			.Line("if (!unknownFields.Add(identifier))")
			.OpenParen()
			.Line("throw DuplicateField(identifier);")
			.CloseParen()
			.Line("_lexer.SkipValueAndSemicolon();")
			.CloseParen()
			.CloseParen()
			.Line()
			.Lines(
				scalarProperties
					.Select((property, index) => (property, index))
					.Where(pair => IsRequiredProperty(pair.property))
					.Select(pair => CreateRequiredCheck(block, pair.property, pair.index))
			)
			.Line($"return new {block.ClassName}(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(CreateModelAssignment))
			.DecreaseIndent()
			.Line(");")
			.CloseParen();
	}

	private static void CreateBlockDispatcher(IndentedWriter output, Block topLevelBlock)
	{
		var collectionProperties = topLevelBlock.Properties.OfType<CollectionProperty>().ToArray();

		output
			.Line("private void AddParsedBlock(")
			.IncreaseIndent()
			.Line("Identifier blockName")
			.DecreaseIndent()
			.Line(")")
			.OpenParen();

		bool first = true;
		foreach (var collectionProperty in collectionProperties)
		{
			var block = UdmfDefinitions.Blocks.Single(b =>
				b.Serialization == SerializationType.Normal && b.ClassName == collectionProperty.ElementTypeName
			);

			output
				.Line($"{(first ? "if" : "else if")} (blockName.EqualsIgnoreCase(\"{block.FormatName}\"))")
				.OpenParen()
				.Line(
					$"{CreateCollectionBuilderFieldName(collectionProperty)}.Add(Parse{collectionProperty.ElementTypeName}Block(blockName));"
				)
				.CloseParen();

			first = false;
		}

		output
			.Line("else")
			.OpenParen()
			.Line("throw new ParsingException($\"Unknown block: {blockName}\");")
			.CloseParen()
			.CloseParen();
	}

	private static void CreateMapDataFactory(IndentedWriter output, Block block)
	{
		output
			.Line("private MapData CreateMapData()")
			.OpenParen()
			.Line("return new MapData(")
			.IncreaseIndent()
			.JoinLines(",", block.OrderedProperties.Select(CreateTopLevelAssignment))
			.DecreaseIndent()
			.Line(");")
			.CloseParen();
	}

	private static void CreateFieldParseBranches(IndentedWriter output, Block block)
	{
		var properties = block.Properties.OfType<ScalarProperty>().ToArray();
		var groups = properties
			.Select((property, index) => (property, index))
			.GroupBy(pair => pair.property.FormatName.Length)
			.OrderBy(group => group.Key);

		foreach (var group in groups)
		{
			output.Line($"case {group.Key}:").IncreaseIndent();

			bool first = true;
			foreach (var (property, index) in group)
			{
				string mask = CreateSeenMask(block, property, index);
				output
					.Line($"{(first ? "if" : "else if")} (identifier.EqualsIgnoreCase(\"{property.FormatName}\"))")
					.OpenParen()
					.Line($"if (HasFlag(seenFields, {mask}))")
					.OpenParen()
					.Line("throw DuplicateField(identifier);")
					.CloseParen()
					.Line($"seenFields |= {mask};")
					.Line($"{property.Name} = {CreateParseAssignmentExpression(property)};")
					.Line("_lexer.ExpectSemicolon();")
					.Line("handledKnownField = true;")
					.CloseParen();

				first = false;
			}

			output
				.Line("else")
				.OpenParen()
				.Line("handledKnownField = false;")
				.CloseParen()
				.Line("break;")
				.DecreaseIndent();
		}
	}

	private static string CreateParseAssignmentExpression(ScalarProperty property) =>
		property switch
		{
			DoubleProperty => "_lexer.ReadDouble()",
			IntegerProperty => "_lexer.ReadInteger()",
			BooleanProperty => "_lexer.ReadBoolean()",
			StringProperty => "_lexer.ReadString()",
			TextureProperty textureProperty =>
				$"ParseTextureFieldValue(optional: {textureProperty.IsOptional.ToString().ToLowerInvariant()})",
			_ => throw new Exception(
				$"Unsupported scalar property type for parser generation: {property.GetType().Name}"
			),
		};

	private static string CreateLocalDeclaration(ScalarProperty property) =>
		$"{property.PropertyType} {property.Name} = {CreateLocalInitialization(property)};";

	private static string CreateLocalInitialization(ScalarProperty property) =>
		property switch
		{
			TextureProperty textureProperty => textureProperty.IsOptional ? "Texture.None" : "default!",
			_ when property.DefaultString != null => property.DefaultString,
			StringProperty => "default!",
			_ => "default",
		};

	private static bool IsRequiredProperty(ScalarProperty property) =>
		property switch
		{
			TextureProperty textureProperty => !textureProperty.IsOptional,
			_ => property.DefaultString == null,
		};

	private static string CreateRequiredCheck(Block block, ScalarProperty property, int index) =>
		$"if (!HasFlag(seenFields, {CreateSeenMask(block, property, index)})) throw MissingRequiredField(blockName, \"{property.FormatName}\");";

	private static string CreateSeenMask(Block block, ScalarProperty property, int index) =>
		$"{CreateSeenFieldsEnumName(block)}.{CreateSeenFieldName(property)}";

	private static string CreateSeenMaskLiteral(int index) => $"1U << {index}";

	private static string CreateSeenFieldsEnumName(Block block) => $"{block.ClassName}Fields";

	private static string CreateSeenFieldName(ScalarProperty property) => property.PropertyName;

	private static void ValidateSeenFieldCount(Block block, int scalarPropertyCount)
	{
		if (scalarPropertyCount > 32)
		{
			throw new InvalidOperationException(
				$"Block '{block.ClassName}' has {scalarPropertyCount} scalar properties. The generated seen-fields enum currently supports up to 32 fields."
			);
		}
	}
}
