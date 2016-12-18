// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;

namespace Tiledriver.Core.FormatModels.Common
{
    [DebuggerDisplay("{_name}")]
    public sealed class Identifier
    {
        private readonly string _name;

        public Identifier(string name)
        {
            Debug.Assert(name != null, "name != null");
            _name = name;
        }

        public override string ToString()
        {
            return _name.ToLowerInvariant();
        }

        public static explicit operator string(Identifier id)
        {
            return id._name;
        }

        #region Equality members

        private bool Equals(Identifier other)
        {
            return string.Equals(_name, other._name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Identifier && Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(_name);
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