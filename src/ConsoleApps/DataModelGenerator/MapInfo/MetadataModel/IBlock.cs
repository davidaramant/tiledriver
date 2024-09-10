// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel;

interface IBlock
{
	string ClassName { get; }
	ImmutableArray<Property> Properties { get; }
	ImmutableArray<Property> Metadata { get; }

	IEnumerable<Property> OrderedProperties =>
		Metadata
			.Concat(Properties)
			.Where(p => !p.HasDefault)
			.Concat(Metadata.Concat(Properties).Where(p => p.HasDefault));
}
