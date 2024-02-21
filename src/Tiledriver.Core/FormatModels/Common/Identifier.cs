// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Diagnostics;

namespace Tiledriver.Core.FormatModels.Common
{
	/// <summary>
	/// A case-insensitive identifier.
	/// </summary>
	[DebuggerDisplay("{" + nameof(_name) + "}")]
	public readonly struct Identifier
	{
		private readonly string _name;

		public Identifier(string name) => _name = name;

		public string ToLower() => _name.ToLowerInvariant();

		public override string ToString() => _name;

		public static explicit operator string(Identifier id) => id._name;

		public static implicit operator Identifier(string name) => new(name);

		#region Equality members

		private bool Equals(Identifier other) =>
			string.Equals(_name, other._name, StringComparison.InvariantCultureIgnoreCase);

		public override bool Equals(object? obj)
		{
			if (obj is null)
				return false;
			return obj is Identifier identifier && Equals(identifier);
		}

		public override int GetHashCode() => StringComparer.InvariantCultureIgnoreCase.GetHashCode(_name);

		public static bool operator ==(Identifier left, Identifier right) => Equals(left, right);

		public static bool operator !=(Identifier left, Identifier right) => !Equals(left, right);

		#endregion
	}
}
