// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Metadata
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string name)
        {
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

        public static string ToPluralCamelCase(this string name)
        {
            return name.ToCamelCase() + "s";
        }

        public static string ToPascalCase(this string name)
        {
            return char.ToUpperInvariant(name[0]) + name.Substring(1);
        }

        public static string ToPluralPascalCase(this string name)
        {
            return name.ToPascalCase() + "s";
        }

        public static string ToFieldName(this string name)
        {
            return "_" + name.ToCamelCase();
        }
    }
}