// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System.Collections.Generic;

namespace Tiledriver.DataModelGenerator.DoomGameInfo;

internal static class ReadOnlyDictionaryExtensions
{
    public static object? TryLookupValue(this IReadOnlyDictionary<string, object> dictionary, string key) =>
        dictionary.TryGetValue(key, out var value) ? value : null;
}
