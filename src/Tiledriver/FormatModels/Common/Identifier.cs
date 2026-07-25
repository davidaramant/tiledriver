using System.Diagnostics;

namespace Tiledriver.FormatModels.Common;

/// <summary>
/// A case-insensitive identifier.
/// </summary>
[DebuggerDisplay("{" + nameof(_name) + "}")]
public readonly struct Identifier : IEquatable<Identifier>
{
	private readonly string _name;
	private static readonly StringComparer Comparer = StringComparer.OrdinalIgnoreCase;

	public Identifier(string name) => _name = name;

	public string ToLower() => _name.ToLowerInvariant();

	public bool EqualsIgnoreCase(string? other) => Comparer.Equals(_name, other);

	public override string ToString() => _name;

	public static explicit operator string(Identifier id) => id._name;

	public static implicit operator Identifier(string name) => new(name);

	#region Equality members

	public bool Equals(Identifier other) => Comparer.Equals(_name, other._name);

	public override bool Equals(object? obj)
	{
		if (obj is null)
			return false;
		return obj is Identifier identifier && Equals(identifier);
	}

	public override int GetHashCode() => Comparer.GetHashCode(_name);

	public static bool operator ==(Identifier left, Identifier right) => Equals(left, right);

	public static bool operator !=(Identifier left, Identifier right) => !Equals(left, right);

	#endregion
}
