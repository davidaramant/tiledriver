// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;

namespace Tiledriver.Metadata
{
    public static class UwmfModelGenerator
    {
        public static string GetText()
        {
            var output = new IndentedWriter();
            output.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.Core.Extensions;

namespace Tiledriver.Core.FormatModels.Uwmf");

            output.OpenParen();
            foreach (var block in UwmfDefinitions.Blocks)
            {
                var normalWriteInheritance = block.NormalWriting ? ", IWriteableUwmfBlock" : String.Empty;
                output.Line($"public sealed partial class {block.ClassName.ToPascalCase()} : BaseUwmfBlock{normalWriteInheritance}");
                output.OpenParen();

                WriteProperties(block, output);
                WriteConstructors(output, block);
                WriteWriteToMethod(block, output);
                WriteSemanticValidityMethods(output, block);

                if (block.SupportsCloning)
                {
                    WriteCloneMethod(output, block);
                }

                output.CloseParen();
                output.Line();
            } // end classes
            output.CloseParen(); // End namespace

            return output.GetString();
        }

        private static void WriteCloneMethod(IndentedWriter output, Block block)
        {
            // TODO: Deep cloning - the unknown properties will point to the same instances
            output.
                Line().
                Line($"public {block.ClassName.ToPascalCase()} Clone()").
                OpenParen().
                Line($"return new {block.ClassName.ToPascalCase()}(").IncreaseIndent();

            foreach (var indexed in block.OrderedProperties().Select((param, index) => new { param, index }))
            {
                var postfix = indexed.index == block.Properties.Count() - 1 ? ");" : ",";

                output.Line(indexed.param.ArgumentName + ": " + indexed.param.PropertyName + postfix);
            }

            output.
                DecreaseIndent().
                CloseParen();
        }

        private static void WriteProperties(Block block, IndentedWriter sb)
        {
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && _.IsRequired))
            {
                sb.Line($"private bool {property.ClassName.ToFieldName()}HasBeenSet = false;").
                    Line($"private {property.PropertyTypeString} {property.ClassName.ToFieldName()};").
                    Line($"public {property.PropertyTypeString} {property.ClassName.ToPascalCase()}").
                    OpenParen().
                    Line($"get {{ return {property.ClassName.ToFieldName()}; }}").
                    Line($"set").
                    OpenParen().
                    Line($"{property.ClassName.ToFieldName()}HasBeenSet = true;").
                    Line($"{property.ClassName.ToFieldName()} = value;").
                    CloseParen().
                    CloseParen();
            }

            foreach (var property in block.Properties.Where(_ => !(_.IsScalarField && _.IsRequired)))
            {
                sb.Line(property.PropertyDefinition);
            }
        }

        private static void WriteConstructors(IndentedWriter sb, Block block)
        {
            sb.Line($"public {block.ClassName.ToPascalCase()}() {{ }}");
            sb.Line($"public {block.ClassName.ToPascalCase()}(");
            sb.IncreaseIndent();

            foreach (var indexed in block.OrderedProperties().Select((param, index) => new { param, index }))
            {
                sb.Line(indexed.param.ArgumentDefinition + (indexed.index == block.Properties.Count() - 1 ? ")" : ","));
            }

            sb.DecreaseIndent();
            sb.OpenParen();

            foreach (var property in block.OrderedProperties())
            {
                sb.Line(property.SetProperty);
            }

            sb.Line(@"AdditionalSemanticChecks();");
            sb.CloseParen();
        }

        private static void WriteWriteToMethod(Block block, IndentedWriter sb)
        {
            if (!block.NormalWriting) return;

            sb.Line(@"public Stream WriteTo(Stream stream)").
                OpenParen().
                Line("CheckSemanticValidity();");

            var indent = block.IsSubBlock ? "true" : "false";

            if (block.IsSubBlock)
            {
                sb.Line($"WriteLine(stream, \"{block.FormatName}\");");
                sb.Line("WriteLine(stream, \"{\");");
            }

            // WRITE ALL REQUIRED PROPERTIES
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && _.IsRequired))
            {
                sb.Line(
                    $"WriteProperty(stream, \"{property.FormatName}\", {property.ClassName.ToFieldName()}, indent: {indent});");
            }
            // WRITE OPTIONAL PROPERTIES
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && !_.IsRequired))
            {
                sb.Line(
                    $"if ({property.ClassName.ToPascalCase()} != {property.DefaultAsString}) WriteProperty(stream, \"{property.FormatName}\", {property.ClassName.ToPascalCase()}, indent: {indent});");
            }

            // WRITE UNKNOWN PROPERTES
            if (block.SupportsUnknownProperties)
            {
                sb.Line($"foreach (var property in UnknownProperties)").
                    OpenParen().
                    Line($"WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: {indent});").
                    CloseParen();
            }

            // WRITE SUBBLOCKS
            foreach (var subBlock in block.Properties.Where(p => p.IsUwmfSubBlockList))
            {
                sb.Line($"WriteBlocks(stream, {subBlock.PropertyName} );");
            }

            if (block.IsSubBlock)
            {
                sb.Line("WriteLine(stream, \"}\");");
            }
            sb.Line("return stream;").
                CloseParen();
        }

        private static void WriteSemanticValidityMethods(IndentedWriter output, Block block)
        {
            output.Line(@"public void CheckSemanticValidity()").
                OpenParen();

            // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
            foreach (var property in block.Properties.Where(_ => _.IsScalarField && _.IsRequired))
            {
                output.Line(
                    $"if (!{property.ClassName.ToFieldName()}HasBeenSet) throw new InvalidUwmfException(\"Did not set {property.ClassName.ToPascalCase()} on {block.ClassName.ToPascalCase()}\");");
            }

            output.Line(@"AdditionalSemanticChecks();").
                CloseParen().
                Line().
                Line("partial void AdditionalSemanticChecks();");
        }
    }
}