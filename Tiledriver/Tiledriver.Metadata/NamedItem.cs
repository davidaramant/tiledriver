// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Metadata
{
    public class NamedItem
    {
        public NamedItem(string uwmfName, string className)
        {
            UwmfName = uwmfName;
            CamelCaseName = className;
        }

        public string UwmfName { get; }
        public string CamelCaseName { get; }
        public string PluralCamelCaseName => CamelCaseName + "s";
        public string PascalCaseName => Char.ToUpperInvariant(CamelCaseName[0]) + CamelCaseName.Substring(1);
        public string PluralPascalCaseName => PascalCaseName + "s";
        public string FieldName => "_" + CamelCaseName;
        public string LowerInvariantName => CamelCaseName.ToLowerInvariant();
    }
}