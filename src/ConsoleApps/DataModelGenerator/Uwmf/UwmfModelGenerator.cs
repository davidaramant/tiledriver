// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using Tiledriver.DataModelGenerator.Utilities;
using Tiledriver.DataModelGenerator.Uwmf.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf
{
    public static class UwmfModelGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            foreach (var block in UwmfDefinitions.Blocks)
            {
                WriteRecord(basePath, block);
            }
        }

        static void WriteRecord(string basePath, Block block)
        {
            using var blockStream =
                File.CreateText(Path.Combine(basePath, block.ClassName + ".Generated.cs"));
            using var output = new IndentedWriter(blockStream);

            output.Line(
                    $@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Uwmf");

            output
                .OpenParen()
                .Line($"[GeneratedCode(\"{CurrentLibraryInfo.Name}\", \"{CurrentLibraryInfo.Version}\")]")
                .Line($"public sealed partial record {block.ClassName}(")
                .IncreaseIndent()
                .JoinLines(",", block.OrderedProperties.Select(GetPropertyDefinition))
                .DecreaseIndent()
                .Line(");")
                .CloseParen();
        }

        static string GetPropertyDefinition(Property property)
        {
            var definition = $"{property.CodeType} {property.CodeName}";

            var defaultString = property.DefaultString;
            if (defaultString!=null)
            {
                definition += $" = {defaultString}";
            }

            return definition;
        }

    }
}