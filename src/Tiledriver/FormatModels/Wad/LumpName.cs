using System.Diagnostics;

namespace Tiledriver.FormatModels.Wad;

[DebuggerDisplay("{ToString()}")]
public sealed class LumpName : IEquatable<LumpName>
{
	public const int MaxLength = 8;
	private readonly string _name;

	public LumpName(string name)
	{
		if (name.Length > MaxLength)
		{
			throw new ArgumentException($"'{name}' is too long.", nameof(name));
		}

		if (name.Any(ch => ch == '\0' || ch > 0x7f))
		{
			throw new ArgumentException($"'{name}' has invalid characters.", nameof(name));
		}

		_name = name;
	}

	public override string ToString() => _name;

	public static implicit operator LumpName(string name) => new(name);

	#region Equality stuff
	public bool Equals(LumpName? other)
	{
		if (other is null)
			return false;
		if (ReferenceEquals(this, other))
			return true;
		return string.Equals(_name, other._name);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null)
			return false;
		if (ReferenceEquals(this, obj))
			return true;
		return obj is LumpName name && Equals(name);
	}

	public override int GetHashCode()
	{
		return _name.GetHashCode();
	}

	public static bool operator ==(LumpName left, LumpName right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(LumpName left, LumpName right)
	{
		return !Equals(left, right);
	}

	#endregion
}
