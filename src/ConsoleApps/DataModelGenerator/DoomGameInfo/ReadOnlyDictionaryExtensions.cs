namespace Tiledriver.DataModelGenerator.DoomGameInfo;

internal static class ReadOnlyDictionaryExtensions
{
	public static object? TryLookupValue(this IReadOnlyDictionary<string, object> dictionary, string key) =>
		dictionary.TryGetValue(key, out var value) ? value : null;
}
