// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;

namespace Tiledriver.UwmfMetadata
{
    public static class ModelGenerator
    {
        public static string GetText()
        {
            var sb = new IndentedWriter();
            sb.Line(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf");

            sb.OpenParen();
            foreach (var block in UwmfDefinitions.Blocks)
            {
                var normalWriteInheritance = block.NormalWriting ? ", IWriteableUwmfBlock" : String.Empty;
                sb.Line($"public sealed partial class {block.PascalCaseName} : BaseUwmfBlock{normalWriteInheritance}");
                sb.OpenParen();

                // Required properties
                foreach (var property in block.Properties.Where(_ => _.IsRequired))
                {
                    sb.Line($"private bool {property.FieldName}HasBeenSet = false;").
                        Line($"private {property.TypeString} {property.FieldName};").
                        Line($"public {property.TypeString} {property.PascalCaseName}").
                        OpenParen().
                        Line($"get {{ return {property.FieldName}; }}").
                        Line($"set").
                        OpenParen().
                        Line($"{property.FieldName}HasBeenSet = true;").
                        Line($"{property.FieldName} = value;").
                        CloseParen().
                        CloseParen();
                }

                // Optional properties
                foreach (var property in block.Properties.Where(_ => !_.IsRequired))
                {
                    sb.Line(
                        $"public {property.TypeString} {property.PascalCaseName} {{ get; set; }} = {property.DefaultAsString};");
                }

                // Sub blocks
                foreach (var subBlock in block.SubBlocks)
                {
                    sb.Line(
                        $"public readonly List<{subBlock.PascalCaseName}> {subBlock.PluralPascalCaseName} = new List<{subBlock.PascalCaseName}>();");
                }

                // Unknown stuff
                if (block.CanHaveUnknownProperties)
                {
                    sb.Line(
                        "public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();");
                }
                if (block.CanHaveUnknownBlocks)
                {
                    sb.Line(
                        "public List<UnknownBlock> UnknownBlocks { get; } = new List<UnknownBlock>();");
                }

                // Constructors
                sb.Line($"public {block.PascalCaseName}() {{ }}");
                sb.Line($"public {block.PascalCaseName}(");
                sb.IncreaseIndent();
                var allParams =
                    block.Properties.
                        Where(p => p.IsRequired).
                        Select(p => $"{p.TypeString} {p.CamelCaseName}").
                        ToList();

                foreach (var subBlock in block.SubBlocks)
                {
                    allParams.Add($"IEnumerable<{subBlock.PascalCaseName}> {subBlock.PluralCamelCaseName}");
                }

                foreach (var p in block.Properties.Where(p => !p.IsRequired))
                {
                    allParams.Add($"{p.TypeString} {p.CamelCaseName}{p.DefaultAssignment}");
                }

                foreach (var indexed in allParams.Select((param, index) => new { param, index }))
                {
                    sb.Line(indexed.param + (indexed.index == allParams.Count - 1 ? ")" : ","));
                }

                sb.DecreaseIndent();
                sb.OpenParen();

                foreach (var property in block.Properties)
                {
                    sb.Line($"{property.PascalCaseName} = {property.CamelCaseName};");
                }

                foreach (var subBlock in block.SubBlocks)
                {
                    sb.Line($"{subBlock.PluralPascalCaseName}.AddRange({subBlock.PluralCamelCaseName});");
                }

                sb.Line(@"AdditionalSemanticChecks();");
                sb.CloseParen(); // End constructor

                // WriteTo
                if (block.NormalWriting)
                {
                    sb.Line(@"public Stream WriteTo(Stream stream)").
                        OpenParen().
                        Line("CheckSemanticValidity();");

                    var indent = block.IsSubBlock ? "true" : "false";

                    if (block.IsSubBlock)
                    {
                        sb.Line($"WriteLine(stream, \"{block.UwmfName}\");");
                        sb.Line("WriteLine(stream, \"{\");");
                    }

                    // WRITE ALL REQUIRED PROPERTIES
                    foreach (var property in block.Properties.Where(_ => _.IsRequired))
                    {
                        sb.Line(
                            $"WriteProperty(stream, \"{property.UwmfName}\", {property.FieldName}, indent: {indent});");
                    }
                    // WRITE OPTIONAL PROPERTIES
                    foreach (var property in block.Properties.Where(_ => !_.IsRequired))
                    {
                        sb.Line(
                            $"if ({property.PascalCaseName} != {property.DefaultAsString}) WriteProperty(stream, \"{property.UwmfName}\", {property.PascalCaseName}, indent: {indent});");
                    }

                    // WRITE UNKNOWN PROPERTES
                    if (block.CanHaveUnknownProperties)
                    {
                        sb.Line($"foreach (var property in UnknownProperties)").
                            OpenParen().
                            Line($"WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: {indent});").
                            CloseParen();
                    }

                    // WRITE SUBBLOCKS
                    foreach (var subBlock in block.SubBlocks)
                    {
                        sb.Line($"WriteBlocks(stream, {subBlock.PluralPascalCaseName} );");
                    }

                    // WRITE UNKNOWN BLOCKS
                    if (block.CanHaveUnknownBlocks)
                    {
                        sb.Line("WriteBlocks(stream, UnknownBlocks);");
                    }
                    if (block.IsSubBlock)
                    {
                        sb.Line("WriteLine(stream, \"}\");");
                    }
                    sb.Line("return stream;").
                        CloseParen();
                } // End 'if' for NormalWriting


                // CheckSemanticValidity
                sb.Line(@"public void CheckSemanticValidity()").
                    OpenParen();

                // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
                foreach (var property in block.Properties.Where(_ => _.IsRequired))
                {
                    sb.Line(
                        $"if (!{property.FieldName}HasBeenSet) throw new InvalidUwmfException(\"Did not set {property.PascalCaseName} on {block.PascalCaseName}\");");
                }

                sb.Line(@"AdditionalSemanticChecks();").
                    CloseParen().
                    Line().
                    Line("partial void AdditionalSemanticChecks();");

                sb.CloseParen(); // end class
                sb.Line();
            } // end classes
            sb.CloseParen(); // End namespace

            return sb.GetString();
        }
    }
}