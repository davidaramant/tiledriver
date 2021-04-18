// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    sealed class PlaneMapsProperty : CollectionProperty
    {
        public override string PropertyType => "ImmutableList<ImmutableArray<TileSpace>>";
        public override string ElementTypeName => "ImmutableArray<TileSpace>";

        public PlaneMapsProperty() : base("planeMap")
        {
        }
    }
}