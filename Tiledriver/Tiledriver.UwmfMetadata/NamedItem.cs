// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.UwmfMetadata
{
    public class NamedItem
    {
        public NamedItem(string uwmfName, string name)
        {
            UwmfName = uwmfName;
            CamelCaseName = name;
        }

        public string UwmfName { get; }
        public string CamelCaseName { get; }
        public string PluralCamelCaseName => UwmfName + "s";
        public string PascalCaseName => Char.ToUpperInvariant(UwmfName[0]) + UwmfName.Substring(1);
        public string PluralPascalCaseName => PascalCaseName + "s";
        public string FieldName => "_" + UwmfName;
        public string LowerInvariantName => UwmfName.ToLowerInvariant();
    }
}