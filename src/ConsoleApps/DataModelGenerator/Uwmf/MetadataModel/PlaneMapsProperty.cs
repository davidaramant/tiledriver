// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel;

sealed class PlaneMapsProperty : CollectionProperty
{
	public override string PropertyType => "ImmutableArray<ImmutableArray<MapSquare>>";
	public override string ElementTypeName => "ImmutableArray<MapSquare>";

	public PlaneMapsProperty()
		: base("planeMap") { }
}
