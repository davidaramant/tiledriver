﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Functional.Maybe;

namespace Tiledriver.Metadata
{
    public static class XlatModelGenerator
    {
        public static string GetText()
        {
            var output = new IndentedWriter();
            output.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.Extensions;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.Xlat");
            output.OpenParen();

            foreach (var block in XlatDefinitions.Blocks)
            {
                output.Line($"public sealed partial class {block.ClassName.ToPascalCase()}");
                output.OpenParen();

                WriteProperties(block, output);
                WriteConstructors(output, block);
                WriteSemanticValidityMethods(output, block);

                output.CloseParen();
                output.Line();
            } // end classes

            output.CloseParen(); // End namespace

            return output.GetString();
        }

        private static void WriteProperties(BlockData blockData, IndentedWriter sb)
        {
            foreach (var property in blockData.Properties.Where(_ => _.ScalarField && _.IsRequired))
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

            foreach (var property in blockData.Properties.Where(_ => !(_.ScalarField && _.IsRequired)))
            {
                sb.Line(property.PropertyDefinition);
            }
        }

        private static void WriteConstructors(IndentedWriter sb, BlockData blockData)
        {
            sb.Line($"public {blockData.ClassName.ToPascalCase()}() {{ }}");
            sb.Line($"public {blockData.ClassName.ToPascalCase()}(");
            sb.IncreaseIndent();

            foreach (var indexed in blockData.OrderedProperties().Select((param, index) => new { param, index }))
            {
                sb.Line(indexed.param.ArgumentDefinition + (indexed.index == blockData.Properties.Count() - 1 ? ")" : ","));
            }

            sb.DecreaseIndent();
            sb.OpenParen();

            foreach (var property in blockData.OrderedProperties())
            {
                sb.Line(property.SetProperty);
            }

            sb.Line(@"AdditionalSemanticChecks();");
            sb.CloseParen();
        }

        private static void WriteSemanticValidityMethods(IndentedWriter output, BlockData blockData)
        {
            output.Line(@"public void CheckSemanticValidity()").
                OpenParen();

            // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
            foreach (var property in blockData.Properties.Where(_ => _.ScalarField && _.IsRequired))
            {
                output.Line(
                    $"if (!{property.ClassName.ToFieldName()}HasBeenSet) throw new InvalidUwmfException(\"Did not set {property.ClassName.ToPascalCase()} on {blockData.ClassName.ToPascalCase()}\");");
            }

            output.Line(@"AdditionalSemanticChecks();").
                CloseParen().
                Line().
                Line("partial void AdditionalSemanticChecks();");
        }
    }
}