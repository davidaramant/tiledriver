// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels;

public sealed class EntryNotFoundException : Exception
{
	public EntryNotFoundException(string path)
		: base(path) { }
}
