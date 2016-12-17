// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Metadata
{
    public enum PropertyType
    {
        IntegerNumber,
        FloatingPointNumber,
        Boolean,
        String,
    }

    public sealed class UwmfProperty : NamedItem
    {
        public PropertyType Type { get; }

        public string TypeString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                        return "bool";
                    case PropertyType.FloatingPointNumber:
                        return "double";
                    case PropertyType.IntegerNumber:
                        return "int";
                    case PropertyType.String:
                        return "string";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        public string TypeName
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.Boolean:
                        return "Boolean";
                    case PropertyType.FloatingPointNumber:
                        return "FloatingPointNumber";
                    case PropertyType.IntegerNumber:
                        return "IntegerNumber";
                    case PropertyType.String:
                        return "String";
                    default:
                        throw new NotImplementedException("Unknown property type.");
                }
            }
        }

        private readonly object _defaultValue;

        public string DefaultAsString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.IntegerNumber:
                    case PropertyType.FloatingPointNumber:
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

        public string DefaultAssignment
        {
            get
            {
                if (IsRequired)
                    return String.Empty;

                return $" = {DefaultAsString}";
            }
        }

        public bool IsRequired => _defaultValue == null;

        public UwmfProperty(
                string name, 
                string uwmfName, 
                PropertyType type, 
                object defaultValue = null) : 
                    base(
                        uwmfName:uwmfName, 
                        name:name)
        {
            Type = type;
            _defaultValue = defaultValue;
        }        
    }
}