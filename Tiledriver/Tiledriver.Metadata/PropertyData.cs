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
        Char,
        Set,
        Block,
        List,
        MappedBlockList,
        UnknownProperties,
        UnknownBlocks,
    }

    public sealed class PropertyData : NamedItem
    {
        private string _collectionType;

        public PropertyType Type { get; }
        public bool IsMetaData { get; }
        public bool HasDefault => _defaultValue != null;

        public string CollectionType
        {
            get {  return _collectionType ?? ClassName.ToPascalCase(); }
            private set { _collectionType = value; }
        }

        public string PropertyTypeString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                        return "bool";
                    case PropertyType.Char:
                        return "char";
                    case PropertyType.Double:
                        return "double";
                    case PropertyType.Integer:
                        return "int";
                    case PropertyType.Ushort:
                        return "ushort";
                    case PropertyType.String:
                        return "string";
                    case PropertyType.Set:
                        return $"HashSet<{CollectionType}>";
                    case PropertyType.Block:
                        return CollectionType ?? ClassName.ToPascalCase();
                    case PropertyType.List:
                        return $"List<{CollectionType}>";
                    case PropertyType.MappedBlockList:
                        return $"Dictionary<ushort,{CollectionType}>";
                    case PropertyType.UnknownProperties:
                        return "List<UnknownProperty>";
                    case PropertyType.UnknownBlocks:
                        return "List<UnknownBlock>";
                    default:
                        throw new NotImplementedException("Unknown property type: " + Type);
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
                    case PropertyType.Char:
                        return "char";
                    case PropertyType.Double:
                        return "double";
                    case PropertyType.Integer:
                        return "int";
                    case PropertyType.Ushort:
                        return "ushort";
                    case PropertyType.String:
                        return "string";
                    case PropertyType.Block:
                        return CollectionType;
                    case PropertyType.Set:
                    case PropertyType.List:
                        return $"IEnumerable<{CollectionType}>";
                    case PropertyType.MappedBlockList:
                        return $"Dictionary<ushort,{CollectionType}>";
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
                    case PropertyType.Char:
                    case PropertyType.Double:
                    case PropertyType.Integer:
                    case PropertyType.Ushort:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return ClassName.ToCamelCase();
                    case PropertyType.List:
                    case PropertyType.Set:
                    case PropertyType.MappedBlockList:
                        return ClassName.ToPluralCamelCase();
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
                    case PropertyType.Char:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return ClassName.ToPascalCase();
                    case PropertyType.Set:
                    case PropertyType.List:
                    case PropertyType.MappedBlockList:
                        return ClassName.ToPluralPascalCase();
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
                    case PropertyType.Char:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                        return $"public {PropertyTypeString} {PropertyName} {{ get; set; }} = {DefaultAsString};";
                    case PropertyType.Set:
                    case PropertyType.Block:
                    case PropertyType.List:
                    case PropertyType.MappedBlockList:
                    case PropertyType.UnknownProperties:
                    case PropertyType.UnknownBlocks:
                        return $"public {PropertyTypeString} {PropertyName} {{ get; }} = new {PropertyTypeString}();";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string ArgumentDefinition => $"{ArgumentTypeString} {ArgumentName}" + (IsRequired ? string.Empty : $" = {DefaultAsString}");

        public string SetProperty
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Char:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return $"{PropertyName} = {ArgumentName};";
                    case PropertyType.Set:
                    case PropertyType.List:
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
            Type == PropertyType.List ||
            Type == PropertyType.UnknownBlocks;

        private readonly object _defaultValue;

        public string DefaultAsString
        {
            get
            {
                if (!IsScalarField)
                {
                    return "null";
                }

                switch (Type)
                {
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.Double:
                        return _defaultValue.ToString();

                    case PropertyType.Boolean:
                        return _defaultValue.ToString().ToLowerInvariant();

                    case PropertyType.String:
                        return "\"" + _defaultValue + "\"";

                    case PropertyType.Char:
                        return "'" + _defaultValue + "'";

                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string DefaultAssignment => IsRequired ? string.Empty : $" = {DefaultAsString}";

        public bool IsRequired
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Char:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return _defaultValue == null;
                    case PropertyType.Set:
                    case PropertyType.List:
                    case PropertyType.MappedBlockList:
                        return true;
                    case PropertyType.UnknownProperties:
                    case PropertyType.UnknownBlocks:
                        return false;
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public bool IsScalarField
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                    case PropertyType.Char:
                    case PropertyType.Double:
                    case PropertyType.Ushort:
                    case PropertyType.Integer:
                    case PropertyType.String:
                    case PropertyType.Block:
                        return true;
                    case PropertyType.Set:
                    case PropertyType.List:
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
                PropertyType type,
                bool isMetaData = false,
                string formatName = null,
                object defaultValue = null,
                string collectionType = null) :
                    base(
                        formatName: formatName ?? name,
                        className: name)
        {
            Type = type;
            IsMetaData = isMetaData;
            _defaultValue = defaultValue;
            CollectionType = collectionType;
        }
    }
}