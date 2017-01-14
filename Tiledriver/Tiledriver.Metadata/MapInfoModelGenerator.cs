// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;

namespace Tiledriver.Metadata
{
    public static class MapInfoModelGenerator
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
using Functional.Maybe;

namespace Tiledriver.Core.FormatModels.MapInfos");
            output.OpenParen();

            foreach (var block in MapInfoDefinitions.Blocks)
            {
                var allInheritance =
                    block.BaseClass.SelectOrElse(className => new[] { className }, () => new string[0])
                        .Concat(block.ImplementedInterfaces).ToArray();

                var inheritance = allInheritance.Any()
                    ? " :" + string.Join(",", allInheritance.Select(s => " " + s))
                    : string.Empty;

                output.Line($"public partial class {block.ClassName.ToPascalCase()}{inheritance}");
                output.OpenParen();

                WriteProperties(block, output);
                WriteConstructors(output, block);
                WriteWithMethods(block, output);

                output.CloseParen();
                output.Line();
            } // end classes

            output.CloseParen(); // End namespace

            return output.GetString();
        }

        private static void WriteProperties(BlockData blockData, IndentedWriter sb)
        {
            foreach (var property in blockData.Properties)
            {
                if (property.ScalarField)
                {
                    var initialValue = property.HasDefault ?
                        $"(({property.ArgumentTypeString}){property.DefaultAsString}).ToMaybe()" :
                        $"Maybe<{property.PropertyTypeString}>.Nothing";

                    sb.Line($"public Maybe<{property.PropertyTypeString}> {property.ClassName.ToPascalCase()} {{ get; }} = {initialValue};");
                }
                else
                {
                    sb.Line($"public {property.ArgumentTypeString} {property.ClassName.ToPluralPascalCase()} {{ get; }} = Enumerable.Empty<{property.CollectionType}>();");
                }
            }
        }

        private static void WriteConstructors(IndentedWriter sb, BlockData blockData)
        {
            var baseClass =
                blockData.BaseClass.Select(
                    name => MapInfoDefinitions.Blocks.Single(b => b.ClassName == name));

            var allProperties = new List<PropertyData>();

            if (baseClass.HasValue)
            {
                allProperties.AddRange(baseClass.Value.Properties);
            }
            allProperties.AddRange(blockData.Properties);

            sb.Line($"public {blockData.ClassName.ToPascalCase()}() {{ }}");

            if (!allProperties.Any())
            {
                return;
            }

            sb.Line($"public {blockData.ClassName.ToPascalCase()}(");
            sb.IncreaseIndent();

            foreach (var indexed in allProperties.Select((param, index) => new { param, index }))
            {
                var argLine = string.Empty;

                if (indexed.param.ScalarField)
                {
                    argLine += $"Maybe<{indexed.param.ArgumentTypeString}> {indexed.param.ArgumentName}";
                }
                else
                {
                    argLine += $"{indexed.param.ArgumentTypeString} {indexed.param.ArgumentName}";
                }

                sb.Line(argLine +
                    (indexed.index == allProperties.Count - 1 ? ")" : ","));
            }
            if (baseClass.HasValue)
            {
                sb.Line(": base(");
                sb.IncreaseIndent();
                foreach (var indexed in baseClass.Value.Properties.Select((param, index) => new { param, index }))
                {
                    sb.Line(indexed.param.ArgumentName +
                        (indexed.index == baseClass.Value.Properties.Count() - 1 ? ")" : ","));
                }
                sb.DecreaseIndent();
            }

            sb.DecreaseIndent();
            sb.OpenParen();

            foreach (var property in blockData.Properties)
            {
                if (property.ScalarField)
                {
                    sb.Line($"{property.ClassName.ToPascalCase()} = {property.ArgumentName};");
                }
                else
                {
                    sb.Line($"{property.ClassName.ToPluralPascalCase()} = {property.ArgumentName};");
                }
            }

            sb.CloseParen();
        }

        private static void WriteWithMethods(BlockData blockData, IndentedWriter sb)
        {

        }
    }
}