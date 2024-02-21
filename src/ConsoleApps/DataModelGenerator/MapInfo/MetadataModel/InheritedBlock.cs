// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Humanizer;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel;

sealed class InheritedBlock : IBlock
{
	private readonly Lazy<AbstractBlock> _baseClass;
	private readonly IReadOnlyList<Property> _metadata;
	private readonly IReadOnlyList<Property> _properties;

	public IBlock BaseClass => _baseClass.Value;
	public string FormatName { get; }
	public string ClassName => FormatName.Pascalize();
	public ImmutableArray<Property> Metadata => _metadata.Concat(BaseClass.Metadata).ToImmutableArray();
	public ImmutableArray<Property> Properties => _properties.Concat(BaseClass.Properties).ToImmutableArray();

	public InheritedBlock(
		string formatName,
		Func<AbstractBlock> getBaseClass,
		IEnumerable<Property> metadata,
		IEnumerable<Property> properties
	)
	{
		FormatName = formatName;
		_metadata = metadata.ToList();
		_properties = properties.ToList();
		_baseClass = new Lazy<AbstractBlock>(getBaseClass);
	}
}
