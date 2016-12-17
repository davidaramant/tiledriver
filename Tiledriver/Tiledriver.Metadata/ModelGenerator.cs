// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;

namespace Tiledriver.Metadata
{
    public static class ModelGenerator
    {
        public static string GetText()
        {
            var output = new IndentedWriter();
            output.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf");

            output.OpenParen();
            foreach (var block in UwmfDefinitions.Blocks)
            {
                var normalWriteInheritance = block.NormalWriting ? ", IWriteableUwmfBlock" : String.Empty;
                output.Line($"public sealed partial class {block.PascalCaseName} : BaseUwmfBlock{normalWriteInheritance}");
                output.OpenParen();

                WriteRequiredProperties(block, output);
                WriteOptionalProperties(block, output);
                WriteSubBlockProperties(block, output);
                WriteUnknownProperties(block, output);
                WriteConstructors(output, block);
                WriteWriteToMethod(block, output);
                WriteSemanticValidityMethods(output, block);

                output.CloseParen();
                output.Line();
            } // end classes
            output.CloseParen(); // End namespace

            return output.GetString();
        }

        private static void WriteRequiredProperties(UwmfBlock block, IndentedWriter sb)
        {
            foreach (var property in block.Properties.Where(_ => _.IsRequired))
            {
                sb.Line($"private bool {property.FieldName}HasBeenSet = false;").
                    Line($"private {property.TypeString} {property.FieldName};").
                    Line($"public {property.TypeString} {property.PascalCaseName}").
                    OpenParen().
                    Line($"get {{ return {property.FieldName}; }}").
                    Line($"set").
                    OpenParen().
                    Line($"{property.FieldName}HasBeenSet = true;").
                    Line($"{property.FieldName} = value;").
                    CloseParen().
                    CloseParen();
            }
        }

        private static void WriteOptionalProperties(UwmfBlock block, IndentedWriter sb)
        {
            foreach (var property in block.Properties.Where(_ => !_.IsRequired))
            {
                sb.Line(
                    $"public {property.TypeString} {property.PascalCaseName} {{ get; set; }} = {property.DefaultAsString};");
            }
        }

        private static void WriteSubBlockProperties(UwmfBlock block, IndentedWriter sb)
        {
            foreach (var subBlock in block.SubBlocks)
            {
                sb.Line(
                    $"public List<{subBlock.PascalCaseName}> {subBlock.PluralPascalCaseName} {{ get; }} = new List<{subBlock.PascalCaseName}>();");
            }
        }

        private static void WriteUnknownProperties(UwmfBlock block, IndentedWriter sb)
        {
            if (block.CanHaveUnknownProperties)
            {
                sb.Line(
                    "public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();");
            }
            if (block.CanHaveUnknownBlocks)
            {
                sb.Line(
                    "public List<UnknownBlock> UnknownBlocks { get; } = new List<UnknownBlock>();");
            }
        }

        private static void WriteConstructors(IndentedWriter sb, UwmfBlock block)
        {
            sb.Line($"public {block.PascalCaseName}() {{ }}");
            sb.Line($"public {block.PascalCaseName}(");
            sb.IncreaseIndent();
            var allParams =
                block.Properties.
                    Where(p => p.IsRequired).
                    Select(p => $"{p.TypeString} {p.CamelCaseName}").
                    ToList();

            foreach (var subBlock in block.SubBlocks)
            {
                allParams.Add($"IEnumerable<{subBlock.PascalCaseName}> {subBlock.PluralCamelCaseName}");
            }

            foreach (var p in block.Properties.Where(p => !p.IsRequired))
            {
                allParams.Add($"{p.TypeString} {p.CamelCaseName}{p.DefaultAssignment}");
            }

            foreach (var indexed in allParams.Select((param, index) => new { param, index }))
            {
                sb.Line(indexed.param + (indexed.index == allParams.Count - 1 ? ")" : ","));
            }

            sb.DecreaseIndent();
            sb.OpenParen();

            foreach (var property in block.Properties)
            {
                sb.Line($"{property.PascalCaseName} = {property.CamelCaseName};");
            }

            foreach (var subBlock in block.SubBlocks)
            {
                sb.Line($"{subBlock.PluralPascalCaseName}.AddRange({subBlock.PluralCamelCaseName});");
            }

            sb.Line(@"AdditionalSemanticChecks();");
            sb.CloseParen();
        }

        private static void WriteWriteToMethod(UwmfBlock block, IndentedWriter sb)
        {
            if (!block.NormalWriting) return;

            sb.Line(@"public Stream WriteTo(Stream stream)").
                OpenParen().
                Line("CheckSemanticValidity();");

            var indent = block.IsSubBlock ? "true" : "false";

            if (block.IsSubBlock)
            {
                sb.Line($"WriteLine(stream, \"{block.UwmfName}\");");
                sb.Line("WriteLine(stream, \"{\");");
            }

            // WRITE ALL REQUIRED PROPERTIES
            foreach (var property in block.Properties.Where(_ => _.IsRequired))
            {
                sb.Line(
                    $"WriteProperty(stream, \"{property.UwmfName}\", {property.FieldName}, indent: {indent});");
            }
            // WRITE OPTIONAL PROPERTIES
            foreach (var property in block.Properties.Where(_ => !_.IsRequired))
            {
                sb.Line(
                    $"if ({property.PascalCaseName} != {property.DefaultAsString}) WriteProperty(stream, \"{property.UwmfName}\", {property.PascalCaseName}, indent: {indent});");
            }

            // WRITE UNKNOWN PROPERTES
            if (block.CanHaveUnknownProperties)
            {
                sb.Line($"foreach (var property in UnknownProperties)").
                    OpenParen().
                    Line($"WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: {indent});").
                    CloseParen();
            }

            // WRITE SUBBLOCKS
            foreach (var subBlock in block.SubBlocks)
            {
                sb.Line($"WriteBlocks(stream, {subBlock.PluralPascalCaseName} );");
            }

            // WRITE UNKNOWN BLOCKS
            if (block.CanHaveUnknownBlocks)
            {
                sb.Line("WriteBlocks(stream, UnknownBlocks);");
            }
            if (block.IsSubBlock)
            {
                sb.Line("WriteLine(stream, \"}\");");
            }
            sb.Line("return stream;").
                CloseParen();
        }

        private static void WriteSemanticValidityMethods(IndentedWriter output, UwmfBlock block)
        {
            output.Line(@"public void CheckSemanticValidity()").
                OpenParen();

            // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
            foreach (var property in block.Properties.Where(_ => _.IsRequired))
            {
                output.Line(
                    $"if (!{property.FieldName}HasBeenSet) throw new InvalidUwmfException(\"Did not set {property.PascalCaseName} on {block.PascalCaseName}\");");
            }

            output.Line(@"AdditionalSemanticChecks();").
                CloseParen().
                Line().
                Line("partial void AdditionalSemanticChecks();");
        }
    }
}