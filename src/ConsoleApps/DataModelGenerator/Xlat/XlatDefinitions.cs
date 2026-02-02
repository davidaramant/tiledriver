// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Xlat.MetadataModel;

namespace Tiledriver.DataModelGenerator.Xlat;

static class XlatDefinitions
{
	public static readonly ImmutableArray<Block> Blocks =
	[
		new Block("elevator", Serialization: SerializationType.Custom, Properties: [new UShortProperty("oldNum")]),
		new Block(
			"thingTemplate",
			Serialization: SerializationType.Custom,
			Properties:
			[
				new UShortProperty("oldNum"),
				new StringProperty("type"),
				new IntegerProperty("angles"),
				new BooleanProperty("holowall"),
				new BooleanProperty("pathing"),
				new BooleanProperty("ambush"),
				new IntegerProperty("minskill"),
			]
		),
		new Block(
			"triggerTemplate",
			Properties:
			[
				new UShortProperty("oldNum"),
				new StringProperty("action"),
				new IntegerProperty("arg0", defaultValue: 0),
				new IntegerProperty("arg1", defaultValue: 0),
				new IntegerProperty("arg2", defaultValue: 0),
				new IntegerProperty("arg3", defaultValue: 0),
				new IntegerProperty("arg4", defaultValue: 0),
				new BooleanProperty("activateEast", defaultValue: true),
				new BooleanProperty("activateNorth", defaultValue: true),
				new BooleanProperty("activateWest", defaultValue: true),
				new BooleanProperty("activateSouth", defaultValue: true),
				new BooleanProperty("playerCross", defaultValue: false),
				new BooleanProperty("playerUse", defaultValue: false),
				new BooleanProperty("monsterUse", defaultValue: false),
				new BooleanProperty("repeatable", defaultValue: false),
				new BooleanProperty("secret", defaultValue: false),
				new StringProperty("comment", defaultValue: string.Empty),
			]
		),
		new Block(
			"ambushModzone",
			Serialization: SerializationType.Custom,
			Properties: [new UShortProperty("oldNum"), new BooleanProperty("fillzone", defaultValue: false)]
		),
		new Block(
			"changeTriggerModzone",
			Serialization: SerializationType.Custom,
			Properties:
			[
				new UShortProperty("oldNum"),
				new BooleanProperty("fillzone", defaultValue: false),
				new StringProperty("action"),
				new BlockProperty("triggerTemplate"),
			]
		),
		new Block(
			"tileTemplate",
			Properties:
			[
				new UShortProperty("oldNum"),
				new StringProperty("textureEast"),
				new StringProperty("textureNorth"),
				new StringProperty("textureWest"),
				new StringProperty("textureSouth"),
				new BooleanProperty("blockingEast", defaultValue: true),
				new BooleanProperty("blockingNorth", defaultValue: true),
				new BooleanProperty("blockingWest", defaultValue: true),
				new BooleanProperty("blockingSouth", defaultValue: true),
				new BooleanProperty("offsetVertical", defaultValue: false),
				new BooleanProperty("offsetHorizontal", defaultValue: false),
				new BooleanProperty("dontOverlay", defaultValue: false),
				new IntegerProperty("mapped", defaultValue: 0),
				new StringProperty("soundSequence", defaultValue: string.Empty),
				new StringProperty("textureOverhead", defaultValue: string.Empty),
				new StringProperty("comment", defaultValue: string.Empty),
			]
		),
		new Block(
			"zoneTemplate",
			Serialization: SerializationType.Custom,
			Properties: [new UShortProperty("oldNum"), new StringProperty("comment", defaultValue: string.Empty)]
		),
		new Block(
			"flats",
			ClassName: "FlatMappings",
			Serialization: SerializationType.TopLevel,
			Properties:
			[
				new ArrayProperty("ceiling", elementType: "string"),
				new ArrayProperty("floor", elementType: "string"),
			]
		),
	];
}
