// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

namespace Tiledriver.Core.Uwmf.Parsing.Extensions
{
    public static class CharExtensions
    {
        public static bool IsValidIdentifierStartChar(this char c)
        {
            return
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                c == '_';
        }

        public static bool IsValidIdentifierChar(this char c)
        {
            return
                IsValidIdentifierStartChar(c) ||
                (c >= '0' && c <= '9');
        }

        public static bool IsStartOfComment(this char c)
        {
            return c == '/';
        }
    }
}