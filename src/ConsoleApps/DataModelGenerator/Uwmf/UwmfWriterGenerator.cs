// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using Tiledriver.DataModelGenerator.Utilities;
using Tiledriver.DataModelGenerator.Uwmf.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf
{
    public static class UwmfWriterGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            using var stream = File.CreateText(Path.Combine(basePath, "UwmfWriter.Generated.cs"));
            using var output = new IndentedWriter(stream);

            output.Line(
                    $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.IO;

namespace Tiledriver.Core.FormatModels.Uwmf.Writing")
                .OpenParen()
                .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
                .Line($"public static partial class UwmfWriter")
                .OpenParen();

            foreach (var block in UwmfDefinitions.Blocks.Where(b => b.Serialization != SerializationType.Custom))
            {
                CreateBlockWriter(output, block);
            }

            output.CloseParen().CloseParen();
        }

        private static void CreateBlockWriter(IndentedWriter output, Block block)
        {
            output
                .Line($"private static void Write(this StreamWriter writer, {block.ClassName} {block.Name})")
                .OpenParen();

            if (block.Serialization == SerializationType.Normal)
            {
                output
                    .Line($"writer.WriteLine(\"{block.Name}\");")
                    .Line("writer.WriteLine(\"{\");");
            }

            foreach (var p in block.Properties)
            {
                if (p is ScalarProperty sp)
                {
                    var defaultValue = sp.DefaultString;

                    string optionalDefault = defaultValue != null ? ", " + defaultValue : string.Empty;

                    output.Line($"WriteProperty(writer, \"{sp.FormatName}\", {block.Name}.{sp.CodeName}{optionalDefault});");
                }
                else if (p is ListProperty lp)
                {
                    output.Line($"foreach(var block in {block.Name}.{lp.CodeName})")
                        .OpenParen()
                        .Line($"writer.Write(block);")
                        .CloseParen();
                }
                else if(p is UnknownBlocks ub)
                {
                    // TODO
                }
                else if (p is UnknownProperties up)
                {
                    // TODO:
                }
            }

            if (block.Serialization == SerializationType.Normal)
            {
                output.Line("writer.WriteLine(\"}\");");
            }
            output.CloseParen();
        }
    }
}