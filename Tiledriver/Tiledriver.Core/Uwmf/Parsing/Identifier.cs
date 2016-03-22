// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Diagnostics;

namespace Tiledriver.Core.Uwmf.Parsing
{
    // TODO: Kill this class and just normalize the names directly.
    [DebuggerDisplay("{Name}")]
    public sealed class Identifier
    {
        public string Name { get; }

        public Identifier(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Matches(string otherName)
        {
            return string.Equals(Name, otherName, StringComparison.OrdinalIgnoreCase);
        }

        #region Equality members

        private bool Equals(Identifier other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Identifier && Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
        }

        public static bool operator ==(Identifier left, Identifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}