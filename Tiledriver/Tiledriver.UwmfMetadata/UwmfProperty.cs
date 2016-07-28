/*
** UwmfProperty.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

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