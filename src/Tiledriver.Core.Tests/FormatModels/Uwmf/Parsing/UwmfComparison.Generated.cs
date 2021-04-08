// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Uwmf;
using Xunit;
using FluentAssertions;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing
{
    public static partial class UwmfComparison
    {
        public static void AssertEqual( Tile actual, Tile expected )
        {
            
                actual.TextureEast.Should().Be( expected.TextureEast );
            
                actual.TextureNorth.Should().Be( expected.TextureNorth );
            
                actual.TextureWest.Should().Be( expected.TextureWest );
            
                actual.TextureSouth.Should().Be( expected.TextureSouth );
            
                actual.BlockingEast.Should().Be( expected.BlockingEast );
            
                actual.BlockingNorth.Should().Be( expected.BlockingNorth );
            
                actual.BlockingWest.Should().Be( expected.BlockingWest );
            
                actual.BlockingSouth.Should().Be( expected.BlockingSouth );
            
                actual.OffsetVertical.Should().Be( expected.OffsetVertical );
            
                actual.OffsetHorizontal.Should().Be( expected.OffsetHorizontal );
            
                actual.DontOverlay.Should().Be( expected.DontOverlay );
            
                actual.Mapped.Should().Be( expected.Mapped );
            
                actual.SoundSequence.Should().Be( expected.SoundSequence );
            
                actual.TextureOverhead.Should().Be( expected.TextureOverhead );
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Sector actual, Sector expected )
        {
            
                actual.TextureCeiling.Should().Be( expected.TextureCeiling );
            
                actual.TextureFloor.Should().Be( expected.TextureFloor );
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Zone actual, Zone expected )
        {
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Plane actual, Plane expected )
        {
            
                actual.Depth.Should().Be( expected.Depth );
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( TileSpace actual, TileSpace expected )
        {
            
                actual.Tile.Should().Be( expected.Tile );
            
                actual.Sector.Should().Be( expected.Sector );
            
                actual.Zone.Should().Be( expected.Zone );
            
                actual.Tag.Should().Be( expected.Tag );
        }
        public static void AssertEqual( PlaneMap actual, PlaneMap expected )
        {
            
                actual.TileSpaces.Count.Should().Be( expected.TileSpaces.Count );
            for( int i = 0; i < expected.TileSpaces.Count; i++ )
            {
                AssertEqual( 
                    actual.TileSpaces[i],
                    expected.TileSpaces[i] );
            }
        }
        public static void AssertEqual( Thing actual, Thing expected )
        {
            
                actual.Type.Should().Be( expected.Type );
            
                actual.X.Should().Be( expected.X );
            
                actual.Y.Should().Be( expected.Y );
            
                actual.Z.Should().Be( expected.Z );
            
                actual.Angle.Should().Be( expected.Angle );
            
                actual.Ambush.Should().Be( expected.Ambush );
            
                actual.Patrol.Should().Be( expected.Patrol );
            
                actual.Skill1.Should().Be( expected.Skill1 );
            
                actual.Skill2.Should().Be( expected.Skill2 );
            
                actual.Skill3.Should().Be( expected.Skill3 );
            
                actual.Skill4.Should().Be( expected.Skill4 );
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( Trigger actual, Trigger expected )
        {
            
                actual.X.Should().Be( expected.X );
            
                actual.Y.Should().Be( expected.Y );
            
                actual.Z.Should().Be( expected.Z );
            
                actual.Action.Should().Be( expected.Action );
            
                actual.Arg0.Should().Be( expected.Arg0 );
            
                actual.Arg1.Should().Be( expected.Arg1 );
            
                actual.Arg2.Should().Be( expected.Arg2 );
            
                actual.Arg3.Should().Be( expected.Arg3 );
            
                actual.Arg4.Should().Be( expected.Arg4 );
            
                actual.ActivateEast.Should().Be( expected.ActivateEast );
            
                actual.ActivateNorth.Should().Be( expected.ActivateNorth );
            
                actual.ActivateWest.Should().Be( expected.ActivateWest );
            
                actual.ActivateSouth.Should().Be( expected.ActivateSouth );
            
                actual.PlayerCross.Should().Be( expected.PlayerCross );
            
                actual.PlayerUse.Should().Be( expected.PlayerUse );
            
                actual.MonsterUse.Should().Be( expected.MonsterUse );
            
                actual.Repeatable.Should().Be( expected.Repeatable );
            
                actual.Secret.Should().Be( expected.Secret );
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
        }
        public static void AssertEqual( MapData actual, MapData expected )
        {
            
                actual.NameSpace.Should().Be( expected.NameSpace );
            
                actual.TileSize.Should().Be( expected.TileSize );
            
                actual.Name.Should().Be( expected.Name );
            
                actual.Width.Should().Be( expected.Width );
            
                actual.Height.Should().Be( expected.Height );
            
                actual.Comment.Should().Be( expected.Comment );
            
                actual.Tiles.Count.Should().Be( expected.Tiles.Count );
            for( int i = 0; i < expected.Tiles.Count; i++ )
            {
                AssertEqual( 
                    actual.Tiles[i],
                    expected.Tiles[i] );
            }
            
                actual.Sectors.Count.Should().Be( expected.Sectors.Count );
            for( int i = 0; i < expected.Sectors.Count; i++ )
            {
                AssertEqual( 
                    actual.Sectors[i],
                    expected.Sectors[i] );
            }
            
                actual.Zones.Count.Should().Be( expected.Zones.Count );
            for( int i = 0; i < expected.Zones.Count; i++ )
            {
                AssertEqual( 
                    actual.Zones[i],
                    expected.Zones[i] );
            }
            
                actual.Planes.Count.Should().Be( expected.Planes.Count );
            for( int i = 0; i < expected.Planes.Count; i++ )
            {
                AssertEqual( 
                    actual.Planes[i],
                    expected.Planes[i] );
            }
            
                actual.PlaneMaps.Count.Should().Be( expected.PlaneMaps.Count );
            for( int i = 0; i < expected.PlaneMaps.Count; i++ )
            {
                AssertEqual( 
                    actual.PlaneMaps[i],
                    expected.PlaneMaps[i] );
            }
            
                actual.Things.Count.Should().Be( expected.Things.Count );
            for( int i = 0; i < expected.Things.Count; i++ )
            {
                AssertEqual( 
                    actual.Things[i],
                    expected.Things[i] );
            }
            
                actual.Triggers.Count.Should().Be( expected.Triggers.Count );
            for( int i = 0; i < expected.Triggers.Count; i++ )
            {
                AssertEqual( 
                    actual.Triggers[i],
                    expected.Triggers[i] );
            }
            
                actual.UnknownProperties.Count.Should().Be( expected.UnknownProperties.Count );
            for( int i = 0; i < expected.UnknownProperties.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownProperties[i],
                    expected.UnknownProperties[i] );
            }
            
                actual.UnknownBlocks.Count.Should().Be( expected.UnknownBlocks.Count );
            for( int i = 0; i < expected.UnknownBlocks.Count; i++ )
            {
                AssertEqual( 
                    actual.UnknownBlocks[i],
                    expected.UnknownBlocks[i] );
            }
        }
    }
}