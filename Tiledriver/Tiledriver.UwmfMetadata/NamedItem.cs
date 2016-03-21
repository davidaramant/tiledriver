// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.UwmfMetadata
{
    public class NamedItem
    {
        public NamedItem(string name)
        {
            CamelCaseName = name;
        }

        public string CamelCaseName { get; }
        public string PascalCaseName => Char.ToUpperInvariant(CamelCaseName[0]) + CamelCaseName.Substring(1);
        public string PluralPascalCaseName => PascalCaseName + "s";
        public string FieldName => "_" + CamelCaseName;
    }
}