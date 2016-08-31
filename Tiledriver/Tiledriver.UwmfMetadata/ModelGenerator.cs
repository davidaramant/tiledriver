// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using System.Text;

namespace Tiledriver.UwmfMetadata
{
    public static class ModelGenerator
    {
        public static string GetText()
        {
            var sb = new StringBuilder();
            sb.AppendLine(
@"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf
{");
            foreach (var block in UwmfDefinitions.Blocks)
            {
                var extraInheritance = block.NormalWriting ? ", IWriteableUwmfBlock" : String.Empty;
                sb.AppendLine($"    public sealed partial class {block.PascalCaseName} : BaseUwmfBlock{extraInheritance}");
                sb.AppendLine("    {");

                // Required properties
                foreach (var property in block.Properties.Where(_ => _.IsRequired))
                {
                    sb.AppendLine($@"
        private bool {property.FieldName}HasBeenSet = false;
        private {property.TypeString} {property.FieldName};
        public {property.TypeString} {property.PascalCaseName}
        {{
            get {{ return {property.FieldName}; }}
            set 
            {{ 
                {property.FieldName}HasBeenSet = true;
                {property.FieldName} = value;
            }}
        }}");
                }

                // Optional properties
                foreach (var property in block.Properties.Where(_ => !_.IsRequired))
                {
                    sb.AppendLine(
                        $"        public {property.TypeString} {property.PascalCaseName} {{ get; set; }} = {property.DefaultAsString};");
                }

                // Sub blocks
                foreach (var subBlock in block.SubBlocks)
                {
                    sb.AppendLine(
                        $"        public readonly List<{subBlock.PascalCaseName}> {subBlock.PluralPascalCaseName} = new List<{subBlock.PascalCaseName}>();");
                }

                // Unknown stuff
                if (block.CanHaveUnknownProperties)
                {
                    sb.AppendLine(
                        "        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();");
                }
                if (block.CanHaveUnknownBlocks)
                {
                    sb.AppendLine(
                        "        public List<UnknownBlock> UnknownBlocks { get; } = new List<UnknownBlock>();");
                }

                // Constructors
                sb.AppendLine($"public {block.PascalCaseName}() {{ }}");
                sb.AppendLine($"public {block.PascalCaseName}(");
                var allParams =
                    block.Properties.
                        Where(p => p.IsRequired).
                        Select(p => $"{new string(' ', 12)}{p.TypeString} {p.CamelCaseName}").
                        ToList();

                foreach (var subBlock in block.SubBlocks)
                {
                    allParams.Add($"{new string(' ', 12)}IEnumerable<{subBlock.PascalCaseName}> {subBlock.PluralCamelCaseName}");
                }

                foreach (var p in block.Properties.Where(p => !p.IsRequired))
                {
                    allParams.Add($"{new string(' ', 12)}{p.TypeString} {p.CamelCaseName}{p.DefaultAssignment}");
                }

                var constructorParams =
                    String.Join(
                        ", " + Environment.NewLine,
                        allParams);
                sb.AppendLine(constructorParams);
                sb.AppendLine(")");
                sb.AppendLine("{");

                foreach (var property in block.Properties)
                {
                    sb.AppendLine($"{property.PascalCaseName} = {property.CamelCaseName};");
                }

                foreach (var subBlock in block.SubBlocks)
                {
                    sb.AppendLine($"{subBlock.PluralPascalCaseName}.AddRange( {subBlock.PluralCamelCaseName} );");
                }

                sb.AppendLine(@"AdditionalSemanticChecks();
}");
                // WriteTo
                if (block.NormalWriting)
                {
                    sb.AppendLine(@"public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();");

                    var indent = block.IsSubBlock ? "true" : "false";

                    if (block.IsSubBlock)
                    {
                        sb.AppendLine($"WriteLine(stream, \"{block.UwmfName}\");");
                        sb.AppendLine("WriteLine(stream, \"{\");");
                    }

                    // WRITE ALL REQUIRED PROPERTIES
                    foreach (var property in block.Properties.Where(_ => _.IsRequired))
                    {
                        sb.AppendLine(
                            $"WriteProperty(stream, \"{property.UwmfName}\", {property.FieldName}, indent: {indent} );");
                    }
                    // WRITE OPTIONAL PROPERTIES
                    foreach (var property in block.Properties.Where(_ => !_.IsRequired))
                    {
                        sb.AppendLine(
                            $"if ( {property.PascalCaseName} != {property.DefaultAsString} ) WriteProperty(stream, \"{property.UwmfName}\", {property.PascalCaseName}, indent: {indent} );");
                    }

                    // WRITE UNKNOWN PROPERTES
                    if (block.CanHaveUnknownProperties)
                    {
                        sb.AppendLine($"foreach (var property in UnknownProperties)");
                        sb.AppendLine("{");
                        sb.AppendLine(
                            $"WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: {indent} );");
                        sb.AppendLine("}");
                    }

                    // WRITE SUBBLOCKS
                    foreach (var subBlock in block.SubBlocks)
                    {
                        sb.AppendLine($"WriteBlocks(stream, {subBlock.PluralPascalCaseName} );");
                    }

                    // WRITE UNKNOWN BLOCKS
                    if (block.CanHaveUnknownBlocks)
                    {
                        sb.AppendLine("WriteBlocks(stream, UnknownBlocks);");
                    }
                    if (block.IsSubBlock)
                    {
                        sb.AppendLine("WriteLine(stream, \"}\");");
                    }
                    sb.AppendLine("return stream;");
                    sb.AppendLine("}");
                } // End 'if' for NormalWriting


                // CheckSemanticValidity
                sb.AppendLine(@"public void CheckSemanticValidity()
        {");

                // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
                foreach (var property in block.Properties.Where(_ => _.IsRequired))
                {
                    sb.AppendLine(
                        $"if (! {property.FieldName}HasBeenSet ) throw new InvalidUwmfException(\"Did not set {property.PascalCaseName} on {block.PascalCaseName}\");");
                }

                sb.AppendLine(@"AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();");

                sb.AppendLine("    }"); // end class
                sb.AppendLine();
            }
            sb.AppendLine("}"); // End namespace


            var a = @"// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf
{
<#
foreach( var block in UwmfDefinitions.Blocks )
{
#>
    public sealed partial class <#= block.PascalCaseName #> : BaseUwmfBlock<# if( block.NormalWriting ) { #>, IWriteableUwmfBlock<# }#> 
    {
<#
    // #######################
    // # REQUIRED PROPERTIES #
    // #######################
    foreach( var property in block.Properties.Where( _ => _.IsRequired ) )
    {
#>
        private bool <#= property.FieldName #>HasBeenSet = false;
        private <#= property.TypeString #> <#= property.FieldName #>;
        public <#= property.TypeString #> <#= property.PascalCaseName #>
        {
            get { return <#= property.FieldName #>; }
            set 
            { 
                <#= property.FieldName #>HasBeenSet = true;
                <#= property.FieldName #> = value;
            }
        }
<#
    }
    // ###########################
    // # END REQUIRED PROPERTIES #
    // ###########################

    // #######################
    // # OPTIONAL PROPERTIES #
    // #######################
    foreach( var property in block.Properties.Where( _ => !_.IsRequired ) )
    {
#>
        public <#= property.TypeString #> <#= property.PascalCaseName #> { get; set; } = <#= property.DefaultAsString #>;
<#
    }
    // ###########################
    // # END OPTIONAL PROPERTIES #
    // ###########################

    // #############
    // # SUBBLOCKS #
    // #############
    foreach( var subBlock in block.SubBlocks )
    {
#>
        public readonly List<<#= subBlock.PascalCaseName #>> <#= subBlock.PluralPascalCaseName #> = new List<<#= subBlock.PascalCaseName #>>();
<#
    }
    // #################
    // # END SUBBLOCKS #
    // #################

    // #################
    // # UNKNOWN STUFF #
    // #################
if( block.CanHaveUnknownProperties )
{
#>
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
<#
}
if( block.CanHaveUnknownBlocks )
{
#>
        public List<UnknownBlock> UnknownBlocks { get; } = new List<UnknownBlock>();
<#
}
    // #####################
    // # END UNKNOWN STUFF #
    // #####################

    // ################
    // # CONSTRUCTORS #
    // ################
#>

        public <#= block.PascalCaseName #>() { }

        public <#= block.PascalCaseName #>(
<#
    var allParams = 
        block.Properties.Where( p=>p.IsRequired ).Select( 
            p => $""{new string(' ', 12)}{p.TypeString} {p.CamelCaseName}"" ).ToList();

    foreach( var subBlock in block.SubBlocks )
    {
        allParams.Add( $""{new string(' ',12)}IEnumerable<{subBlock.PascalCaseName}> {subBlock.PluralCamelCaseName}"" );
    }

    foreach (var p in block.Properties.Where(p => !p.IsRequired) )
    {
        allParams.Add($""{new string(' ', 12)}{p.TypeString} {p.CamelCaseName}{p.DefaultAssignment}"");
    }


    var constructorParams = 
        String.Join( 
            "","" + Environment.NewLine, 
            allParams );
#>
<#= constructorParams #>)
        {
<#
    foreach( var property in block.Properties )
    {
#>
            <#= property.PascalCaseName #> = <#= property.CamelCaseName #>;
<#
    }
    foreach( var subBlock in block.SubBlocks )
    {
#>
            <#= subBlock.PluralPascalCaseName #>.AddRange( <#= subBlock.PluralCamelCaseName #> );
<#
    }
#>

            AdditionalSemanticChecks();
        }
<#
    // ####################
    // # END CONSTRUCTORS #
    // ####################

    // ##################
    // # WRITETO METHOD #
    // ##################

    if( block.NormalWriting ) 
    { 
#>

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

<#
    var indent = block.IsSubBlock ? ""true"" : ""false"";

    if( block.IsSubBlock )
    {
#>
            WriteLine( stream, ""<#= block.UwmfName #>"");
            WriteLine( stream, ""{"");
<#
    }

    // WRITE ALL REQUIRED PROPERTIES
    foreach( var property in block.Properties.Where( _ => _.IsRequired ) )
    {
#>
            WriteProperty( stream, ""<#= property.UwmfName #>"", <#= property.FieldName #>, indent: <#= indent #> );
<#
    }
    // WRITE OPTIONAL PROPERTIES
    foreach( var property in block.Properties.Where( _ => !_.IsRequired ) )
    {
#>
            if( <#= property.PascalCaseName #> != <#= property.DefaultAsString #> )
            {
                WriteProperty( stream, ""<#= property.UwmfName #>"", <#= property.PascalCaseName #>, indent: <#= indent #> );
            }
<#
    }
    // WRITE UNKNOWN PROPERTES
    if( block.CanHaveUnknownProperties )
    {
#>
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: <#= indent #> );
            }
<#
    }
    // WRITE SUBBLOCKS
    foreach( var subBlock in block.SubBlocks )
    {
#>
            WriteBlocks( stream,  <#= subBlock.PluralPascalCaseName #> );
<#
    }
    // WRITE UNKNOWN BLOCKS
    if( block.CanHaveUnknownBlocks )
    {
#>
            WriteBlocks( stream,  UnknownBlocks );
<#
    }
    if( block.IsSubBlock )
    {
#>
            WriteLine( stream, ""}"");
<#
    }
#>
                
            return stream;
        }
<#
    } // End 'if' for NormalWriting
    // ######################
    // # END WRITETO METHOD #
    // ######################

    // #########################
    // # CHECKSEMANTICVALIDITY #
    // #########################
#>

        public void CheckSemanticValidity()
        {
<#
    // CHECK THAT ALL REQUIRED PROPERTIES HAVE BEEN SET
    foreach( var property in block.Properties.Where( _ => _.IsRequired ) )
    {
#>
            if( ! <#= property.FieldName #>HasBeenSet )
            {
                throw new InvalidUwmfException(""Did not set <#= property.PascalCaseName #> on <#= block.PascalCaseName #>"");
            }
<#
    }
#>
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

<#
}
    // #############################
    // # END CHECKSEMANTICVALIDITY #
    // #############################
#>
}";
            return sb.ToString();
        }
    }
}