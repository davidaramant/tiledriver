// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Metadata
{
    public enum PropertyType
    {
        Integer,
        Ushort,
        Double,
        Boolean,
        String,
        UshortSet,
        StringList,
        Block,
        BlockList,
        MappedBlockList,
        UnknownProperties,
        UnknownBlocks,
    }

    public sealed class PropertyData : NamedItem
    {
        public PropertyType Type { get; }

        public string PropertyTypeString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                        return "bool";
                    case PropertyType.Double:
                        return "double";
                    case PropertyType.Integer:
                        return "int";
                    case PropertyType.Ushort:
                        return "ushort";
                    case PropertyType.String:
                        return "string";
                    case PropertyType.UshortSet:
                        return "HashSet<ushort>";
                    case PropertyType.StringList:
                        return "List<string>";
                    case PropertyType.Block:
                        return PascalCaseName;
                    case PropertyType.BlockList:
                        return $"List<{PascalCaseName}>";
                    case PropertyType.MappedBlockList:
                        return $"Dictionary<ushort,{PascalCaseName}>";
                    case PropertyType.UnknownProperties:
                        return "List<UnknownProperty>";
                    case PropertyType.UnknownBlocks:
                        return "List<UnknownBlock>";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string ArgumentTypeString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                        return "bool";
                    case PropertyType.Double:
                        return "double";
                    case PropertyType.Integer:
                        return "int";
                    case PropertyType.Ushort:
                        return "ushort";
                    case PropertyType.String:
                        return "string";
                    case PropertyType.UshortSet:
                        return "IEnumerable<ushort>";
                    case PropertyType.StringList:
                        return "IEnumerable<string>";
                    case PropertyType.Block:
                        return PascalCaseName;
                    case PropertyType.BlockList:
                        return $"IEnumerable<{PascalCaseName}>";
                    case PropertyType.MappedBlockList:
                        return $"Dictionary<ushort,{PascalCaseName}>";
                    case PropertyType.UnknownProperties:
                        return "IEnumerable<UnknownProperty>";
                    case PropertyType.UnknownBlocks:
                        return "IEnumerable<UnknownBlock>";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string ArgumentName
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Double:
                    case PropertyType.Integer:
                    case PropertyType.Ushort:
                    case PropertyType.String:
                    case PropertyType.UshortSet:
                    case PropertyType.StringList:
                    case PropertyType.Block:
                        return CamelCaseName;
                    case PropertyType.BlockList:
                    case PropertyType.MappedBlockList:
                        return PluralCamelCaseName;
                    case PropertyType.UnknownProperties:
                        return "unknownProperties";
                    case PropertyType.UnknownBlocks:
                        return "unknownBlocks";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string PropertyName
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.UshortSet:
                    case PropertyType.StringList:
                    case PropertyType.Block:
                        return PascalCaseName;
                    case PropertyType.BlockList:
                    case PropertyType.MappedBlockList:
                        return PluralPascalCaseName;
                    case PropertyType.UnknownProperties:
                        return "UnknownProperties";
                    case PropertyType.UnknownBlocks:
                        return "UnknownBlocks";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string PropertyDefinition
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                        return $"public {PropertyTypeString} {PropertyName} {{ get; set; }} = {DefaultAsString};";
                    case PropertyType.UshortSet:
                    case PropertyType.StringList:
                    case PropertyType.Block:
                    case PropertyType.BlockList:
                    case PropertyType.MappedBlockList:
                    case PropertyType.UnknownProperties:
                    case PropertyType.UnknownBlocks:
                        return $"public {PropertyTypeString} {PropertyName} {{ get; }} = new {PropertyTypeString}();";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string ArgumentDefinition => $"{ArgumentTypeString} {ArgumentName}" + (IsRequired?string.Empty:$" = {DefaultAsString}");

        public string SetProperty
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return $"{PropertyName} = {ArgumentName};";
                    case PropertyType.UshortSet:
                    case PropertyType.StringList:
                    case PropertyType.BlockList:
                    case PropertyType.MappedBlockList:
                        return $"{PropertyName}.AddRange({ArgumentName});";
                    case PropertyType.UnknownProperties:
                        return $"{PropertyName}.AddRange({ArgumentName} ?? Enumerable.Empty<UnknownProperty>());";
                    case PropertyType.UnknownBlocks:
                        return $"{PropertyName}.AddRange({ArgumentName} ?? Enumerable.Empty<UnknownBlock>());";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }


        public string UwmfTypeMethod
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                        return "Boolean";
                    case PropertyType.Double:
                        return "FloatingPointNumber";
                    case PropertyType.Integer:
                        return "IntegerNumber";
                    case PropertyType.String:
                        return "String";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public bool IsUwmfSubBlockList => 
            Type == PropertyType.BlockList || 
            Type == PropertyType.UnknownBlocks;

        private readonly object _defaultValue;

        public string DefaultAsString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.Double:
                    case PropertyType.UnknownProperties:
                    case PropertyType.UnknownBlocks:
                        return _defaultValue.ToString();

                    case PropertyType.Boolean:
                        return _defaultValue.ToString().ToLowerInvariant();

                    case PropertyType.String:
                        return "\"" + _defaultValue + "\"";                   

                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string DefaultAssignment => IsRequired ? string.Empty : $" = {DefaultAsString}";

        public bool IsRequired => _defaultValue == null;

        public bool ScalarField
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return true;
                    case PropertyType.UshortSet:
                    case PropertyType.StringList:
                    case PropertyType.BlockList:
                    case PropertyType.MappedBlockList:
                    case PropertyType.UnknownProperties:
                    case PropertyType.UnknownBlocks:
                        return false;
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public PropertyData(
                string name, 
                string uwmfName, 
                PropertyType type, 
                object defaultValue = null) : 
                    base(
                        uwmfName:uwmfName, 
                        className:name)
        {
            Type = type;
            _defaultValue = defaultValue;
        }        
    }
}