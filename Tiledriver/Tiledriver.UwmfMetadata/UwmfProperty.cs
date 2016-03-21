// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.UwmfMetadata
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

        public int DefaultIntegerNumber => GetDefaultValue<int>(PropertyType.IntegerNumber);
        public double DefaultFloatingPointNumber => GetDefaultValue<double>(PropertyType.FloatingPointNumber);
        public string DefaultString => GetDefaultValue<string>(PropertyType.String);
        public bool DefaultBoolean => GetDefaultValue<bool>(PropertyType.Boolean);

        public bool IsRequired => _defaultValue == null;

        public UwmfProperty(string name, PropertyType type, object defaultValue = null) : base(name)
        {
            Type = type;
            _defaultValue = defaultValue;
        }

        private T GetDefaultValue<T>(PropertyType expectedType)
        {
            if (IsRequired)
            {
                throw new InvalidOperationException("This value is required and does not have a default.");
            }
            if (Type != expectedType)
            {
                throw new InvalidOperationException($"This value is a {Type}, not a {expectedType}.");
            }
            return (T)_defaultValue;
        }

        public static UwmfProperty RequiredIntegerNumber(string name)
        {
            return new UwmfProperty(name, PropertyType.IntegerNumber, defaultValue: null);
        }

        public static UwmfProperty RequiredFloatingPointNumber(string name)
        {
            return new UwmfProperty(name, PropertyType.FloatingPointNumber, defaultValue: null);
        }

        public static UwmfProperty RequiredString(string name)
        {
            return new UwmfProperty(name, PropertyType.String, defaultValue: null);
        }

        public static UwmfProperty RequiredBoolean(string name)
        {
            return new UwmfProperty(name, PropertyType.Boolean, defaultValue: null);
        }

        public static UwmfProperty OptionalIntegerNumber(string name, int defaultValue)
        {
            return new UwmfProperty(name, PropertyType.IntegerNumber, defaultValue: defaultValue);
        }

        public static UwmfProperty OptionalFloatingPointNumber(string name, double defaultValue)
        {
            return new UwmfProperty(name, PropertyType.FloatingPointNumber, defaultValue: defaultValue);
        }

        public static UwmfProperty OptionalString(string name, string defaultValue)
        {
            return new UwmfProperty(name, PropertyType.String, defaultValue: defaultValue);
        }

        public static UwmfProperty OptionalBoolean(string name, bool defaultValue)
        {
            return new UwmfProperty(name, PropertyType.Boolean, defaultValue: defaultValue);
        }
    }
}