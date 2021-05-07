﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo
{
    public static class MapInfoModelGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            foreach (var block in MapInfoDefinitions.Blocks)
            {
                WriteRecord(basePath, block);
            }
        }

        static void WriteRecord(string basePath, IBlock block)
        {
            using var blockStream =
                File.CreateText(Path.Combine(basePath, block.ClassName + ".Generated.cs"));
            using var output = new IndentedWriter(blockStream);

            var containsCollection = block.OrderedProperties.Any(p => p is CollectionProperty);
            var containsIdentifier = block.OrderedProperties.Any(p => p is IdentifierProperty);

            List<string> includes = new() { "System.CodeDom.Compiler" };
            if (containsCollection)
            {
                includes.Add("System.Collections.Immutable");
            }

            if (containsIdentifier)
            {
                includes.Add("Tiledriver.Core.FormatModels.Common");
            }

            output
                .WriteHeader("Tiledriver.Core.FormatModels.MapInfo", includes)
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