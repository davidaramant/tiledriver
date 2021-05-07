// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Humanizer;

namespace Tiledriver.DataModelGenerator.Utilities
{
    public static class StringExtensions
    {
        public static string ToFieldName(this string name) => "_" + name.Camelize();
    }
}