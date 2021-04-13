// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Tiledriver.DataModelGenerator.Utilities;
using Tiledriver.DataModelGenerator.Uwmf.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf
{
    public static class UwmfSemanticAnalyzerGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            using var stream = File.CreateText(Path.Combine(basePath, "UwmfSemanticAnalyzer.Generated.cs"));
            using var output = new IndentedWriter(stream);

            output.Line(
                    $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading")
                .OpenParen()
                .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
                .Line($"public static partial class UwmfSemanticAnalyzer")
                .OpenParen();

            foreach (var block in UwmfDefinitions.Blocks.Where(b => b.Serialization == SerializationType.Normal))
            {
                CreateBlockReader(output, block);
            }

            CreateGlobalBlockReader(output, UwmfDefinitions.Blocks.Single(b => b.Serialization == SerializationType.TopLevel));

            output.CloseParen().CloseParen();
        }

        private static string CreateParameterAssignment(Property property, string context = "block.Name") => property switch
        {
            ScalarProperty sp => CreateParameterAssignment(sp, context),
            CollectionProperty cp => CreateParameterAssignment(cp),
            _ => throw new Exception("Unknown property type"),
        };

        private static string CreateParameterAssignment(ScalarProperty property, string context = "block.Name")
        {
            var getValue = property.DefaultString == null
                ? $"GetRequiredFieldValue<{property.CodeType}>(fields, {context}, \"{property.FormatName}\")"
                : $"GetOptionalFieldValue<{property.CodeType}>(fields, \"{property.FormatName}\")";

            if (property is DoubleProperty)
            {
                getValue = property.DefaultString == null
                    ? $"GetRequiredDoubleFieldValue(fields, {context}, \"{property.FormatName}\")"
                    : $"GetOptionalDoubleFieldValue(fields, \"{property.FormatName}\")";
            }

            return $"{property.CodeName}: {getValue}";
        }

        private static string CreateParameterAssignment(CollectionProperty property)
        {
            return $"{property.CodeName}: {property.Name}Builder.ToImmutable()";
        }

        private static void CreateBlockReader(IndentedWriter output, Block block)
        {
            output
                .Line($"private static {block.ClassName} Read{block.ClassName}(Block block)")
                .OpenParen()
                .Line("var fields = block.GetFieldAssignments();")
                .Line()
                .Line($"return new {block.ClassName}(")
                .IncreaseIndent()
                .JoinLines(",", block.OrderedProperties.Select(p => CreateParameterAssignment(p)))
                .DecreaseIndent()
                .Line(");")
                .CloseParen();
        }

        private static void CreateGlobalBlockReader(IndentedWriter output, Block block)
        {
            output
                .Line($"public static {block.ClassName} Read{block.ClassName}(IEnumerable<IGlobalExpression> ast)")
                .OpenParen()
                .Line("Dictionary<Identifier, Token> fields = new();")
                .Line("var block = new IdentifierToken(FilePosition.StartOfFile, \"MapData\");")
                .Lines(block.Properties.OfType<CollectionProperty>().Select(cp =>
                    $"var {cp.Name}Builder = ImmutableList.CreateBuilder<{cp.GenericTypeName}>();"))
                .Line()
                .Line("foreach(var expression in ast)")
                .OpenParen()
                .Line("switch (expression)")
                .OpenParen()
                .Line("case Assignment a:").IncreaseIndent()
                .Line("fields.Add(a.Name.Id, a.Value);")
                .Line("break;").DecreaseIndent()
                .Line()
                .Line("case Block b:").IncreaseIndent()
                .Line("switch (b.Name.Id.ToLower())")
                .OpenParen();

            foreach (var cp in block.Properties.OfType<CollectionProperty>().Where(p => p.Name != "planeMap"))
            {
                output.Line($"case \"{cp.FormatName}\":").IncreaseIndent()
                    .Line($"{cp.Name}Builder.Add(Read{cp.GenericTypeName}(b));")
                    .Line("break;").DecreaseIndent();
            }

            output
                .Line("default:").IncreaseIndent()
                .Line("throw new ParsingException($\"Unknown block: {b.Name}\");").DecreaseIndent()
                .CloseParen()
                .Line("break;").DecreaseIndent()
                .Line()
                .Line("case IntTupleBlock itb:").IncreaseIndent()
                .Line("if (itb.Name.Id.ToLower() != \"planemap\")")
                .OpenParen()
                .Line("throw new ParsingException(\"Unknown int tuple block\");")
                .CloseParen()
                .Line("planeMapBuilder.Add(ReadPlaneMap(itb));")
                .Line("break;").DecreaseIndent()
                .Line()
                .Line("default:").IncreaseIndent()
                .Line("throw new ParsingException(\"Unknown expression type\");").DecreaseIndent()
                .CloseParen()
                .CloseParen()
                .Line()
                .Line($"return new {block.ClassName}(")
                .IncreaseIndent()
                .JoinLines(",", block.OrderedProperties.Select(p => CreateParameterAssignment(p, context: "block")))
                .DecreaseIndent()
                .Line(");")
                .CloseParen();
        }
    }
}