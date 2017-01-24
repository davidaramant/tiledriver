// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;

namespace Tiledriver.Metadata
{
    public static class MapInfoParserGenerator
    {
        public static string GetText()
        {
            var output = new IndentedWriter();
            output.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing");
            output.OpenParen();

            output.Line($"public static partial class MapInfoParser");
            output.OpenParen();

            WriteBlockParsers(output);



            output.CloseParen(); // end class
            output.CloseParen(); // End namespace

            return output.GetString();
        }

        private static void WriteBlockParsers(IndentedWriter output)
        {
            foreach (var block in MapInfoDefinitions.Blocks.Where(b => b.NormalReading))
            {
                var className = block.ClassName.ToPascalCase();
                var instance = block.ClassName.ToCamelCase();

                output.Line($"private static {className} Parse{className}(MapInfoBlock block)");
                output.OpenParen();

                output.Line($"var {instance} =  {className}.Default;");

                var properties = MapInfoDefinitions.GetAllPropertiesOf(block).ToArray();

                if (properties.Any(p => p.IsMetaData))
                {
                    output.Line($"{instance} = Parse{className}Metadata({instance}, block );");
                }
                else
                {
                    output.Line($"block.AssertMetadataLength(0, \"{className}\");");
                }
                
                output.Line("foreach(var property in block.Children)");
                output.OpenParen();

                output.Line("switch (property.Name.ToLower())");
                output.OpenParen();

                foreach (var property in properties.Where(p=>!p.IsMetaData))
                {
                    output.Line($"case \"{property.FormatName}\":");
                    output.IncreaseIndent();

                    var withMethodName = property.AllowMultiple ? "Additional" + property.SingularName : property.PropertyName;
                    var parsingName = property.AllowMultiple ? property.SingularName : property.ParsingTypeName;

                    output.Line($"{instance} = {instance}.With{withMethodName}( " +
                                $"Parse{parsingName}(property, \"{className} {property.FormatName}\") );");

                    output.Line("break;");
                    output.DecreaseIndent();
                }

                output.Line("default:");
                output.IncreaseIndent();
                output.Line($"throw new ParsingException($\"Unknown property {{property.Name}} found in {className}.\");");
                output.DecreaseIndent();

                output.CloseParen(); // end switch

                output.CloseParen(); // end foreach

                output.Line($"return {instance};");

                output.CloseParen();
                output.Line();
            }
        }
    }
}