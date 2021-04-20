﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.DataModelGenerator.Utilities;
using Tiledriver.DataModelGenerator.Xlat.MetadataModel;

namespace Tiledriver.DataModelGenerator.Xlat
{
    public static class XlatModelGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            foreach (var block in XlatDefinitions.Blocks)
            {
                WriteRecord(basePath, block);
            }
        }

        static void WriteRecord(string basePath, Block block)
        {
            using var blockStream =
                File.CreateText(Path.Combine(basePath, block.ClassName + ".Generated.cs"));
            using var output = new IndentedWriter(blockStream);

            var containsCollection = block.Properties.Any(p => p is CollectionProperty);

            IEnumerable<string> includes = new[] { "System.CodeDom.Compiler" };
            if (containsCollection)
            {
                includes = includes.Concat(new[] { "System.Collections.Immutable" });
            }

            output
                .WriteHeader("Tiledriver.Core.FormatModels.Xlat", includes)
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
            var definition = $"{property.PropertyType} {property.PropertyName}";

            var defaultString = property.DefaultString;
            if (defaultString != null)
            {
                definition += $" = {defaultString}";
            }

            return definition;
        }
    }
}