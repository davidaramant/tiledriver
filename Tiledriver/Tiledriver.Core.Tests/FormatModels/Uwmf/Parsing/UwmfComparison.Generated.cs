// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Uwmf;
using NUnit.Framework;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing
{
    public static partial class UwmfComparison
    {
        public static void AssertEqual( Tile actual, Tile expected )
        {
            Assert.That(
                actual.TextureEast,
                Is.EqualTo( expected.TextureEast ),
                "Found difference in Tile TextureEast" );
            Assert.That(
                actual.TextureNorth,
                Is.EqualTo( expected.TextureNorth ),
                "Found difference in Tile TextureNorth" );
            Assert.That(
                actual.TextureWest,
                Is.EqualTo( expected.TextureWest ),
                "Found difference in Tile TextureWest" );
            Assert.That(
                actual.TextureSouth,
                Is.EqualTo( expected.TextureSouth ),
                "Found difference in Tile TextureSouth" );
            Assert.That(
                actual.BlockingEast,
                Is.EqualTo( expected.BlockingEast ),
                "Found difference in Tile BlockingEast" );
            Assert.That(
                actual.BlockingNorth,
                Is.EqualTo( expected.BlockingNorth ),
                "Found difference in Tile BlockingNorth" );
            Assert.That(
                actual.BlockingWest,
                Is.EqualTo( expected.BlockingWest ),
                "Found difference in Tile BlockingWest" );
            Assert.That(
                actual.BlockingSouth,
                Is.EqualTo( expected.BlockingSouth ),
                "Found difference in Tile BlockingSouth" );
            Assert.That(
                actual.OffsetVertical,
                Is.EqualTo( expected.OffsetVertical ),
                "Found difference in Tile OffsetVertical" );
            Assert.That(
                actual.OffsetHorizontal,
                Is.EqualTo( expected.OffsetHorizontal ),
                "Found difference in Tile OffsetHorizontal" );
            Assert.That(
                actual.DontOverlay,
                Is.EqualTo( expected.DontOverlay ),
                "Found difference in Tile DontOverlay" );
            Assert.That(
                actual.Mapped,
                Is.EqualTo( expected.Mapped ),
                "Found difference in Tile Mapped" );
            Assert.That(
                actual.SoundSequence,
                Is.EqualTo( expected.SoundSequence ),
                "Found difference in Tile SoundSequence" );
            Assert.That(
                actual.TextureOverhead,
                Is.EqualTo( expected.TextureOverhead ),
                "Found difference in Tile TextureOverhead" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Tile Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Tile UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Sector actual, Sector expected )
        {
            Assert.That(
                actual.TextureCeiling,
                Is.EqualTo( expected.TextureCeiling ),
                "Found difference in Sector TextureCeiling" );
            Assert.That(
                actual.TextureFloor,
                Is.EqualTo( expected.TextureFloor ),
                "Found difference in Sector TextureFloor" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Sector Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Sector UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Zone actual, Zone expected )
        {
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Zone Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Zone UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Plane actual, Plane expected )
        {
            Assert.That(
                actual.Depth,
                Is.EqualTo( expected.Depth ),
                "Found difference in Plane Depth" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Plane Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Plane UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( TileSpace actual, TileSpace expected )
        {
            Assert.That(
                actual.Tile,
                Is.EqualTo( expected.Tile ),
                "Found difference in TileSpace Tile" );
            Assert.That(
                actual.Sector,
                Is.EqualTo( expected.Sector ),
                "Found difference in TileSpace Sector" );
            Assert.That(
                actual.Zone,
                Is.EqualTo( expected.Zone ),
                "Found difference in TileSpace Zone" );
            Assert.That(
                actual.Tag,
                Is.EqualTo( expected.Tag ),
                "Found difference in TileSpace Tag" );
        }
        public static void AssertEqual( PlaneMap actual, PlaneMap expected )
        {
            Assert.That(
                actual.TileSpaces.Count,
                Is.EqualTo( expected.TileSpaces.Count ),
                "Found unequal number of PlaneMap TileSpaces" );
            for( int i = 0; i < expected.TileSpaces.Count; i++ )
            {
                AssertEqual( 
                    actual.TileSpaces[i],
                    expected.TileSpaces[i] );
            }
        }
        public static void AssertEqual( Thing actual, Thing expected )
        {
            Assert.That(
                actual.Type,
                Is.EqualTo( expected.Type ),
                "Found difference in Thing Type" );
            Assert.That(
                actual.X,
                Is.EqualTo( expected.X ),
                "Found difference in Thing X" );
            Assert.That(
                actual.Y,
                Is.EqualTo( expected.Y ),
                "Found difference in Thing Y" );
            Assert.That(
                actual.Z,
                Is.EqualTo( expected.Z ),
                "Found difference in Thing Z" );
            Assert.That(
                actual.Angle,
                Is.EqualTo( expected.Angle ),
                "Found difference in Thing Angle" );
            Assert.That(
                actual.Ambush,
                Is.EqualTo( expected.Ambush ),
                "Found difference in Thing Ambush" );
            Assert.That(
                actual.Patrol,
                Is.EqualTo( expected.Patrol ),
                "Found difference in Thing Patrol" );
            Assert.That(
                actual.Skill1,
                Is.EqualTo( expected.Skill1 ),
                "Found difference in Thing Skill1" );
            Assert.That(
                actual.Skill2,
                Is.EqualTo( expected.Skill2 ),
                "Found difference in Thing Skill2" );
            Assert.That(
                actual.Skill3,
                Is.EqualTo( expected.Skill3 ),
                "Found difference in Thing Skill3" );
            Assert.That(
                actual.Skill4,
                Is.EqualTo( expected.Skill4 ),
                "Found difference in Thing Skill4" );
            Assert.That(
                actual.Skill5,
                Is.EqualTo( expected.Skill5 ),
                "Found difference in Thing Skill5" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Thing Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Thing UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Trigger actual, Trigger expected )
        {
            Assert.That(
                actual.X,
                Is.EqualTo( expected.X ),
                "Found difference in Trigger X" );
            Assert.That(
                actual.Y,
                Is.EqualTo( expected.Y ),
                "Found difference in Trigger Y" );
            Assert.That(
                actual.Z,
                Is.EqualTo( expected.Z ),
                "Found difference in Trigger Z" );
            Assert.That(
                actual.Action,
                Is.EqualTo( expected.Action ),
                "Found difference in Trigger Action" );
            Assert.That(
                actual.Arg0,
                Is.EqualTo( expected.Arg0 ),
                "Found difference in Trigger Arg0" );
            Assert.That(
                actual.Arg1,
                Is.EqualTo( expected.Arg1 ),
                "Found difference in Trigger Arg1" );
            Assert.That(
                actual.Arg2,
                Is.EqualTo( expected.Arg2 ),
                "Found difference in Trigger Arg2" );
            Assert.That(
                actual.Arg3,
                Is.EqualTo( expected.Arg3 ),
                "Found difference in Trigger Arg3" );
            Assert.That(
                actual.Arg4,
                Is.EqualTo( expected.Arg4 ),
                "Found difference in Trigger Arg4" );
            Assert.That(
                actual.ActivateEast,
                Is.EqualTo( expected.ActivateEast ),
                "Found difference in Trigger ActivateEast" );
            Assert.That(
                actual.ActivateNorth,
                Is.EqualTo( expected.ActivateNorth ),
                "Found difference in Trigger ActivateNorth" );
            Assert.That(
                actual.ActivateWest,
                Is.EqualTo( expected.ActivateWest ),
                "Found difference in Trigger ActivateWest" );
            Assert.That(
                actual.ActivateSouth,
                Is.EqualTo( expected.ActivateSouth ),
                "Found difference in Trigger ActivateSouth" );
            Assert.That(
                actual.PlayerCross,
                Is.EqualTo( expected.PlayerCross ),
                "Found difference in Trigger PlayerCross" );
            Assert.That(
                actual.PlayerUse,
                Is.EqualTo( expected.PlayerUse ),
                "Found difference in Trigger PlayerUse" );
            Assert.That(
                actual.MonsterUse,
                Is.EqualTo( expected.MonsterUse ),
                "Found difference in Trigger MonsterUse" );
            Assert.That(
                actual.Repeatable,
                Is.EqualTo( expected.Repeatable ),
                "Found difference in Trigger Repeatable" );
            Assert.That(
                actual.Secret,
                Is.EqualTo( expected.Secret ),
                "Found difference in Trigger Secret" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in Trigger Comment" );
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of Trigger UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( MapData actual, MapData expected )
        {
            Assert.That(
                actual.NameSpace,
                Is.EqualTo( expected.NameSpace ),
                "Found difference in MapData NameSpace" );
            Assert.That(
                actual.TileSize,
                Is.EqualTo( expected.TileSize ),
                "Found difference in MapData TileSize" );
            Assert.That(
                actual.Name,
                Is.EqualTo( expected.Name ),
                "Found difference in MapData Name" );
            Assert.That(
                actual.Width,
                Is.EqualTo( expected.Width ),
                "Found difference in MapData Width" );
            Assert.That(
                actual.Height,
                Is.EqualTo( expected.Height ),
                "Found difference in MapData Height" );
            Assert.That(
                actual.Comment,
                Is.EqualTo( expected.Comment ),
                "Found difference in MapData Comment" );
            Assert.That(
                actual.Tiles.Count,
                Is.EqualTo( expected.Tiles.Count ),
                "Found unequal number of MapData Tiles" );
            for( int i = 0; i < expected.Tiles.Count; i++ )
            {
                AssertEqual( 
                    actual.Tiles[i],
                    expected.Tiles[i] );
            }
            Assert.That(
                actual.Sectors.Count,
                Is.EqualTo( expected.Sectors.Count ),
                "Found unequal number of MapData Sectors" );
            for( int i = 0; i < expected.Sectors.Count; i++ )
            {
                AssertEqual( 
                    actual.Sectors[i],
                    expected.Sectors[i] );
            }
            Assert.That(
                actual.Zones.Count,
                Is.EqualTo( expected.Zones.Count ),
                "Found unequal number of MapData Zones" );
            for( int i = 0; i < expected.Zones.Count; i++ )
            {
                AssertEqual( 
                    actual.Zones[i],
                    expected.Zones[i] );
            }
            Assert.That(
                actual.Planes.Count,
                Is.EqualTo( expected.Planes.Count ),
                "Found unequal number of MapData Planes" );
            for( int i = 0; i < expected.Planes.Count; i++ )
            {
                AssertEqual( 
                    actual.Planes[i],
                    expected.Planes[i] );
            }
            Assert.That(
                actual.PlaneMaps.Count,
                Is.EqualTo( expected.PlaneMaps.Count ),
                "Found unequal number of MapData PlaneMaps" );
            for( int i = 0; i < expected.PlaneMaps.Count; i++ )
            {
                AssertEqual( 
                    actual.PlaneMaps[i],
                    expected.PlaneMaps[i] );
            }
            Assert.That(
                actual.Things.Count,
                Is.EqualTo( expected.Things.Count ),
                "Found unequal number of MapData Things" );
            for( int i = 0; i < expected.Things.Count; i++ )
            {
                AssertEqual( 
                    actual.Things[i],
                    expected.Things[i] );
            }
            Assert.That(
                actual.Triggers.Count,
                Is.EqualTo( expected.Triggers.Count ),
                "Found unequal number of MapData Triggers" );
            for( int i = 0; i < expected.Triggers.Count; i++ )
            {
                AssertEqual( 
                    actual.Triggers[i],
                    expected.Triggers[i] );
            }
            Assert.That(
                actual.UnknownProperties.Count,
                Is.EqualTo( expected.UnknownProperties.Count ),
                "Found unequal number of MapData UnknownProperties" );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
            Assert.That(
                actual.UnknownBlocks.Count,
                Is.EqualTo( expected.UnknownBlocks.Count ),
                "Found unequal number of MapData UnknownBlocks" );
            for( int i = 0; i < expected.UnknownBlocks.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownBlocks[i],
                    expected.UnknownBlocks[i] );
            }
        }
    }
}