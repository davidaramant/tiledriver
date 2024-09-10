// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels;

public interface IResourceProvider : IDisposable
{
	/// <summary>
	/// Looks up a resource path.
	/// </summary>
	/// <param name="path">The path to the resource.</param>
	/// <returns>The stream for the resource.</returns>
	/// <exception cref="EntryNotFoundException"/>
	Stream Lookup(string path);

	Stream? TryLookup(string path);
}
