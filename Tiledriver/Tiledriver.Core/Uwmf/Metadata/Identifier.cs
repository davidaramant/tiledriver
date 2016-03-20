// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Diagnostics;

namespace Tiledriver.Core.Uwmf.Metadata
{
    [DebuggerDisplay("{_name}")]
    public sealed class Identifier
    {
        private readonly string _name;

        public Identifier(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        #region Equality members

        private bool Equals(Identifier other)
        {
            return string.Equals(_name, other._name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Identifier && Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return (_name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(_name) : 0);
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