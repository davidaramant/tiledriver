// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;

namespace Tiledriver.Metadata
{
    public static class ParserGenerator
    {
        public static string GetText()
        {
            var output = new IndentedWriter();
            output.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.Uwmf.Parsing.Syntax;

namespace Tiledriver.Core.Uwmf.Parsing").
OpenParen().
Line("public static partial class Parser").
OpenParen();

            WriteGlobalAssignmentParsing(output);
            WriteBlockParsing(output);

            foreach (var block in UwmfDefinitions.Blocks.Where(_ => _.NormalReading))
            {
                output.
                    Line($"private static {block.PascalCaseName} Parse{block.PascalCaseName}(Block block)").
                    OpenParen().
                    Line($"var parsedBlock = new {block.PascalCaseName}();");

                WritePropertyAssignments(block, output, assignmentHolder: "block", owner: "parsedBlock");

                output.Line("return parsedBlock;").CloseParen();
            }

            output.CloseParen();
            return output.CloseParen().GetString();
        }

        private static void WritePropertyAssignments(UwmfBlock block, IndentedWriter output, string assignmentHolder, string owner)
        {
            foreach (var property in block.Properties)
            {
                var level = property.IsRequired ? "Required" : "Optional";

                output.Line(
                    $"Set{level}{property.TypeName}(" +
                    $"{assignmentHolder}.GetValueFor(\"{property.PascalCaseName}\"), " +
                    $"value => {owner}.{property.PascalCaseName} = value, " +
                    $"\"{block.PascalCaseName}\", " +
                    $"\"{property.PascalCaseName}\");");
            }
        }

        private static void WriteGlobalAssignmentParsing(IndentedWriter output)
        {
            output.
                Line("static partial void SetGlobalAssignments(Map map, UwmfSyntaxTree tree)").
                OpenParen();

            WritePropertyAssignments(
                UwmfDefinitions.Blocks.Single(b => b.PascalCaseName == "Map"),
                output, assignmentHolder: "tree", owner: "map");

            output.CloseParen();
        }

        private static void WriteBlockParsing(IndentedWriter output)
        {
            output.
                Line("static partial void SetBlocks(Map map, UwmfSyntaxTree tree)").
                OpenParen();

            foreach (var block in UwmfDefinitions.Blocks.Where(_ => _.NormalReading))
            {
                output.Line($"var {block.CamelCaseName}Name = new Identifier(\"{block.CamelCaseName}\");");
            }

            output.
                Line("foreach (var block in tree.Blocks)").
                OpenParen();

            foreach (var block in UwmfDefinitions.Blocks.Where(_ => _.NormalReading))
            {
                output.Line($"if (block.Name == {block.CamelCaseName}Name) map.{block.PluralPascalCaseName}.Add(Parse{block.PascalCaseName}(block));");
            }

            output.CloseParen().CloseParen();
        }
    }
}