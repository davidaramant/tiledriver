// Copyright (c) 2016, David Aramant
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
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.Xlat");
            output.OpenParen();

            foreach (var block in XlatDefinitions.Blocks)
            {
                var allInheritance =
                    block.BaseClass.SelectOrElse(className => new[] { className }, () => new string[0])
                        .Concat(block.ImplementedInterfaces).ToArray();

                var inheritance = allInheritance.Any()
                    ? " :" + string.Join(",", allInheritance.Select(s => " " + s))
                    : string.Empty;

                output.Line($"public sealed partial class {block.ClassName.ToPascalCase()}{inheritance}");
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