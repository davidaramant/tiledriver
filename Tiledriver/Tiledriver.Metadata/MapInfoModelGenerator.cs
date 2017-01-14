// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
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
                    block.BaseClass.SelectOrElse(className => new[] {className}, () => new string[0])
                        .Concat(block.ImplementedInterfaces).ToArray();

                var inheritance = allInheritance.Any()
                    ? " :" + string.Join(",", allInheritance.Select(s => " " + s))
                    : string.Empty;

                output.Line($"public partial class {block.ClassName.ToPascalCase()}{inheritance}");
                output.OpenParen();

                WriteProperties(block, output);
                //WriteConstructors(output, block);
                //WriteWriteToMethod(block, output);
                //WriteSemanticValidityMethods(output, block);

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
                    sb.Line($"public Maybe<{property.PropertyTypeString}> {property.ClassName.ToPascalCase()} {{ get; private set; }}");
                }
                else
                {
                    sb.Line(property.PropertyDefinition);
                }
            }
        }
    }
}