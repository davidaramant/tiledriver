// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the last characters from the string.
        /// </summary>
        /// <param name="s">The string to operate on.</param>
        /// <param name="count">How many characters to remove from the end.</param>
        /// <returns>The shortened string.</returns>
        public static string RemoveLast(this string s, int count)
        {
            return s.Substring(0, s.Length - count);
        }
    }
}