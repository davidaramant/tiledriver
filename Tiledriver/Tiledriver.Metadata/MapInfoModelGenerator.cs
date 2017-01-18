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
using System.Collections.Immutable;
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

                var type = block.IsAbstract ? "abstract" : "sealed";

                output.Line($"public {type} partial class {block.ClassName.ToPascalCase()}{inheritance}");
                output.OpenParen();

                WriteProperties(block, output);
                WriteConstructors(output, block);
                WritePropertyMutators(block, output);
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
                if (property.IsScalarField)
                {
                    var initialValue = property.HasDefault ?
                        $"(({property.ArgumentTypeString}){property.DefaultAsString}).ToMaybe()" :
                        $"Maybe<{property.PropertyTypeString}>.Nothing";

                    sb.Line($"public Maybe<{property.PropertyTypeString}> {property.PropertyName} {{ get; }} = {initialValue};");
                }
                else
                {
                    sb.Line(property.PropertyDefinition);
                }
            }
        }

        private static void WriteConstructors(IndentedWriter sb, BlockData blockData)
        {
            if (!blockData.IsAbstract)
            {
                sb.Line(
                    $"public static {blockData.ClassName.ToPascalCase()} Default = new {blockData.ClassName.ToPascalCase()}();");
                sb.Line($"private {blockData.ClassName.ToPascalCase()}() {{ }}");
            }
            else
            {
                sb.Line($"protected {blockData.ClassName.ToPascalCase()}() {{ }}");
            }

            var baseClass =
                blockData.BaseClass.Select(
                    name => MapInfoDefinitions.Blocks.Single(b => b.ClassName == name));

            var allProperties = new List<PropertyData>();

            if (baseClass.HasValue)
            {
                allProperties.AddRange(baseClass.Value.Properties);
            }
            allProperties.AddRange(blockData.Properties);

            if (!allProperties.Any())
            {
                return;
            }

            var visibility = blockData.IsAbstract ? "protected" : "public";
            sb.Line($"{visibility} {blockData.ClassName.ToPascalCase()}(");
            sb.IncreaseIndent();

            foreach (var indexed in allProperties.Select((param, index) => new { param, index }))
            {
                var argLine = string.Empty;

                if (indexed.param.IsScalarField)
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
                sb.Line(property.SetProperty);
            }

            sb.CloseParen();
        }

        private static void WritePropertyMutators(BlockData blockData, IndentedWriter sb)
        {
            if (blockData.IsAbstract)
                return;

            var allProperties = GetAllPropertiesOf(blockData);

            if (!allProperties.Any())
                return;

            foreach (var property in allProperties)
            {
                sb.Line($"public {blockData.ClassName.ToPascalCase()} With{property.PropertyName}( {property.ArgumentTypeString} {property.ArgumentName} )");
                sb.OpenParen();
                sb.Line($"return new {blockData.ClassName.ToPascalCase()}(");
                sb.IncreaseIndent();
                foreach (var indexed in allProperties.Select((param, index) => new { param, index }))
                {
                    var argLine = $"{indexed.param.ArgumentName}: ";

                    if (indexed.param == property)
                    {
                        if (indexed.param.IsScalarField)
                        {
                            argLine += $"{indexed.param.ArgumentName}.ToMaybe()";
                        }
                        else
                        {
                            argLine += $"{indexed.param.ArgumentName}";
                        }
                    }
                    else
                    {
                        argLine += indexed.param.PropertyName;
                    }

                    sb.Line(argLine +
                        (indexed.index == allProperties.Count - 1 ? ");" : ","));
                }
                sb.DecreaseIndent();
                sb.CloseParen();

                // For collection types, also add a way to add a single item as a convenience
                if (!property.IsScalarField)
                {
                    var singularPropName = property.SingularName.ToPascalCase();
                    var singularArgName = property.SingularName.ToCamelCase();
                    sb.Line($"public {blockData.ClassName.ToPascalCase()} WithAdditional{singularPropName}( {property.CollectionType} {singularArgName} )");
                    sb.OpenParen();
                    sb.Line($"return new {blockData.ClassName.ToPascalCase()}(");
                    sb.IncreaseIndent();
                    foreach (var indexed in allProperties.Select((param, index) => new { param, index }))
                    {
                        var argLine = $"{indexed.param.ArgumentName}: ";

                        if (indexed.param == property)
                        {
                            argLine += $"{property.PropertyName}.Add({singularArgName})";
                        }
                        else
                        {
                            argLine += indexed.param.PropertyName;
                        }

                        sb.Line(argLine +
                            (indexed.index == allProperties.Count - 1 ? ");" : ","));
                    }
                    sb.DecreaseIndent();
                    sb.CloseParen();
                }
            }
        }

        private static void WriteWithMethods(BlockData blockData, IndentedWriter sb)
        {
            if (!blockData.SetsPropertiesFrom.Any())
            {
                return;
            }

            var allProperties = GetAllPropertiesOf(blockData);

            foreach (var otherBlock in
                blockData.SetsPropertiesFrom.Select(name => MapInfoDefinitions.Blocks.Single(b => b.ClassName == name)))
            {
                var otherType = otherBlock.ClassName.ToPascalCase();
                var otherClassname = otherBlock.ClassName.ToCamelCase();
                var allOtherProperties = new HashSet<string>(GetAllPropertiesOf(otherBlock).Select(p => p.PropertyName));

                sb.Line($"public {blockData.ClassName.ToPascalCase()} With{otherType}( {otherType} {otherClassname} )");
                sb.OpenParen();
                sb.Line($"return new {blockData.ClassName.ToPascalCase()}(");
                sb.IncreaseIndent();
                foreach (var indexed in allProperties.Select((param, index) => new { param, index }))
                {
                    var argLine = $"{indexed.param.ArgumentName}: ";
                    var currentName = indexed.param.PropertyName;

                    if (allOtherProperties.Contains(currentName))
                    {
                        if (indexed.param.IsScalarField)
                        {
                            argLine += $"{otherClassname}.{currentName}.Or({currentName})";
                        }
                        else
                        {
                            argLine += $"{currentName}.AddRange({otherClassname}.{currentName})";
                        }
                    }
                    else
                    {
                        argLine += indexed.param.PropertyName;
                    }

                    sb.Line(argLine +
                        (indexed.index == allProperties.Count - 1 ? ");" : ","));
                }
                sb.DecreaseIndent();
                sb.CloseParen();
            }
        }

        private static List<PropertyData> GetAllPropertiesOf(string className)
        {
            return GetAllPropertiesOf(MapInfoDefinitions.Blocks.Single(b => b.ClassName == className));
        }

        private static List<PropertyData> GetAllPropertiesOf(BlockData block)
        {
            var allProperties = new List<PropertyData>();

            if (block.BaseClass.HasValue)
            {
                allProperties.AddRange(GetAllPropertiesOf(block.BaseClass.Value));
            }
            allProperties.AddRange(block.Properties);

            return allProperties;
        }
    }
}