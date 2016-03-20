// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Core.Uwmf
{
    public sealed class Tile : IUwmfEntry
    {
        private bool _textureEastHasBeenSet = false;
        private string _textureEast;
        private bool _textureNorthHasBeenSet = false;
        private string _textureNorth;
        private bool _textureWestHasBeenSet = false;
        private string _textureWest;
        private bool _textureSouthHasBeenSet = false;
        private string _textureSouth;
        private bool _blockingEastHasBeenSet = false;
        private bool _blockingEast = true;
        private bool _blockingNorthHasBeenSet = false;
        private bool _blockingNorth = true;
        private bool _blockingWestHasBeenSet = false;
        private bool _blockingWest = true;
        private bool _blockingSouthHasBeenSet = false;
        private bool _blockingSouth = true;
        private bool _offsetVerticalHasBeenSet = false;
        private bool _offsetVertical = false;
        private bool _offsetHorizontalHasBeenSet = false;
        private bool _offsetHorizontal = false;
        private bool _dontOverlayHasBeenSet = false;
        private bool _dontOverlay = false;
        private bool _mappedHasBeenSet = false;
        private int _mapped = 0;
        private bool _soundSequenceHasBeenSet = false;
        private string _soundSequence = "";
        private bool _textureOverheadHasBeenSet = false;
        private string _textureOverhead = "";

        public string TextureEast
        {
            get { return _textureEast; }
            set 
			{ 
				_textureEastHasBeenSet = true;
				_textureEast = value;
			}
        }
        public string TextureNorth
        {
            get { return _textureNorth; }
            set 
			{ 
				_textureNorthHasBeenSet = true;
				_textureNorth = value;
			}
        }
        public string TextureWest
        {
            get { return _textureWest; }
            set 
			{ 
				_textureWestHasBeenSet = true;
				_textureWest = value;
			}
        }
        public string TextureSouth
        {
            get { return _textureSouth; }
            set 
			{ 
				_textureSouthHasBeenSet = true;
				_textureSouth = value;
			}
        }
        public bool BlockingEast
        {
            get { return _blockingEast; }
            set 
			{ 
				_blockingEastHasBeenSet = true;
				_blockingEast = value;
			}
        }
        public bool BlockingNorth
        {
            get { return _blockingNorth; }
            set 
			{ 
				_blockingNorthHasBeenSet = true;
				_blockingNorth = value;
			}
        }
        public bool BlockingWest
        {
            get { return _blockingWest; }
            set 
			{ 
				_blockingWestHasBeenSet = true;
				_blockingWest = value;
			}
        }
        public bool BlockingSouth
        {
            get { return _blockingSouth; }
            set 
			{ 
				_blockingSouthHasBeenSet = true;
				_blockingSouth = value;
			}
        }
        public bool OffsetVertical
        {
            get { return _offsetVertical; }
            set 
			{ 
				_offsetVerticalHasBeenSet = true;
				_offsetVertical = value;
			}
        }
        public bool OffsetHorizontal
        {
            get { return _offsetHorizontal; }
            set 
			{ 
				_offsetHorizontalHasBeenSet = true;
				_offsetHorizontal = value;
			}
        }
        public bool DontOverlay
        {
            get { return _dontOverlay; }
            set 
			{ 
				_dontOverlayHasBeenSet = true;
				_dontOverlay = value;
			}
        }
        public int Mapped
        {
            get { return _mapped; }
            set 
			{ 
				_mappedHasBeenSet = true;
				_mapped = value;
			}
        }
        public string SoundSequence
        {
            get { return _soundSequence; }
            set 
			{ 
				_soundSequenceHasBeenSet = true;
				_soundSequence = value;
			}
        }
        public string TextureOverhead
        {
            get { return _textureOverhead; }
            set 
			{ 
				_textureOverheadHasBeenSet = true;
				_textureOverhead = value;
			}
        }

		public Stream WriteTo(Stream stream)
        {
			if( ! _textureEastHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureEast on Tile");
			}
			if( ! _textureNorthHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureNorth on Tile");
			}
			if( ! _textureWestHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureWest on Tile");
			}
			if( ! _textureSouthHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureSouth on Tile");
			}

            return stream.
                Line("tile").
                Line("{").
				Attribute( "textureEast", _textureEast ).
				Attribute( "textureNorth", _textureNorth ).
				Attribute( "textureWest", _textureWest ).
				Attribute( "textureSouth", _textureSouth ).
				MaybeAttribute( _mapped != 0, "mapped", _mapped ).
				MaybeAttribute( _blockingEast != true, "blockingEast", _blockingEast ).
				MaybeAttribute( _blockingNorth != true, "blockingNorth", _blockingNorth ).
				MaybeAttribute( _blockingWest != true, "blockingWest", _blockingWest ).
				MaybeAttribute( _blockingSouth != true, "blockingSouth", _blockingSouth ).
				MaybeAttribute( _offsetVertical != false, "offsetVertical", _offsetVertical ).
				MaybeAttribute( _offsetHorizontal != false, "offsetHorizontal", _offsetHorizontal ).
				MaybeAttribute( _dontOverlay != false, "dontOverlay", _dontOverlay ).
				MaybeAttribute( _soundSequence != "", "soundSequence", _soundSequence ).
				MaybeAttribute( _textureOverhead != "", "textureOverhead", _textureOverhead ).
                Line("}");		
		}
    }

    public sealed class Sector : IUwmfEntry
    {
        private bool _textureCeilingHasBeenSet = false;
        private string _textureCeiling;
        private bool _textureFloorHasBeenSet = false;
        private string _textureFloor;

        public string TextureCeiling
        {
            get { return _textureCeiling; }
            set 
			{ 
				_textureCeilingHasBeenSet = true;
				_textureCeiling = value;
			}
        }
        public string TextureFloor
        {
            get { return _textureFloor; }
            set 
			{ 
				_textureFloorHasBeenSet = true;
				_textureFloor = value;
			}
        }

		public Stream WriteTo(Stream stream)
        {
			if( ! _textureCeilingHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureCeiling on Sector");
			}
			if( ! _textureFloorHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureFloor on Sector");
			}

            return stream.
                Line("sector").
                Line("{").
				Attribute( "textureCeiling", _textureCeiling ).
				Attribute( "textureFloor", _textureFloor ).
                Line("}");		
		}
    }

    public sealed class Zone : IUwmfEntry
    {


		public Stream WriteTo(Stream stream)
        {

            return stream.
                Line("zone").
                Line("{").
                Line("}");		
		}
    }

    public sealed class Plane : IUwmfEntry
    {
        private bool _depthHasBeenSet = false;
        private int _depth;

        public int Depth
        {
            get { return _depth; }
            set 
			{ 
				_depthHasBeenSet = true;
				_depth = value;
			}
        }

		public Stream WriteTo(Stream stream)
        {
			if( ! _depthHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Depth on Plane");
			}

            return stream.
                Line("plane").
                Line("{").
				Attribute( "depth", _depth ).
                Line("}");		
		}
    }

    public sealed class Thing : IUwmfEntry
    {
        private bool _typeHasBeenSet = false;
        private int _type;
        private bool _xHasBeenSet = false;
        private double _x;
        private bool _yHasBeenSet = false;
        private double _y;
        private bool _zHasBeenSet = false;
        private double _z;
        private bool _angleHasBeenSet = false;
        private int _angle;
        private bool _ambushHasBeenSet = false;
        private bool _ambush = false;
        private bool _patrolHasBeenSet = false;
        private bool _patrol = false;
        private bool _skill1HasBeenSet = false;
        private bool _skill1 = false;
        private bool _skill2HasBeenSet = false;
        private bool _skill2 = false;
        private bool _skill3HasBeenSet = false;
        private bool _skill3 = false;
        private bool _skill4HasBeenSet = false;
        private bool _skill4 = false;
        private bool _skill5HasBeenSet = false;
        private bool _skill5 = false;

        public int Type
        {
            get { return _type; }
            set 
			{ 
				_typeHasBeenSet = true;
				_type = value;
			}
        }
        public double X
        {
            get { return _x; }
            set 
			{ 
				_xHasBeenSet = true;
				_x = value;
			}
        }
        public double Y
        {
            get { return _y; }
            set 
			{ 
				_yHasBeenSet = true;
				_y = value;
			}
        }
        public double Z
        {
            get { return _z; }
            set 
			{ 
				_zHasBeenSet = true;
				_z = value;
			}
        }
        public int Angle
        {
            get { return _angle; }
            set 
			{ 
				_angleHasBeenSet = true;
				_angle = value;
			}
        }
        public bool Ambush
        {
            get { return _ambush; }
            set 
			{ 
				_ambushHasBeenSet = true;
				_ambush = value;
			}
        }
        public bool Patrol
        {
            get { return _patrol; }
            set 
			{ 
				_patrolHasBeenSet = true;
				_patrol = value;
			}
        }
        public bool Skill1
        {
            get { return _skill1; }
            set 
			{ 
				_skill1HasBeenSet = true;
				_skill1 = value;
			}
        }
        public bool Skill2
        {
            get { return _skill2; }
            set 
			{ 
				_skill2HasBeenSet = true;
				_skill2 = value;
			}
        }
        public bool Skill3
        {
            get { return _skill3; }
            set 
			{ 
				_skill3HasBeenSet = true;
				_skill3 = value;
			}
        }
        public bool Skill4
        {
            get { return _skill4; }
            set 
			{ 
				_skill4HasBeenSet = true;
				_skill4 = value;
			}
        }
        public bool Skill5
        {
            get { return _skill5; }
            set 
			{ 
				_skill5HasBeenSet = true;
				_skill5 = value;
			}
        }

		public Stream WriteTo(Stream stream)
        {
			if( ! _typeHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Type on Thing");
			}
			if( ! _xHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set X on Thing");
			}
			if( ! _yHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Y on Thing");
			}
			if( ! _zHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Z on Thing");
			}
			if( ! _angleHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Angle on Thing");
			}

            return stream.
                Line("thing").
                Line("{").
				Attribute( "type", _type ).
				Attribute( "x", _x ).
				Attribute( "y", _y ).
				Attribute( "z", _z ).
				Attribute( "angle", _angle ).
				MaybeAttribute( _ambush != false, "ambush", _ambush ).
				MaybeAttribute( _patrol != false, "patrol", _patrol ).
				MaybeAttribute( _skill1 != false, "skill1", _skill1 ).
				MaybeAttribute( _skill2 != false, "skill2", _skill2 ).
				MaybeAttribute( _skill3 != false, "skill3", _skill3 ).
				MaybeAttribute( _skill4 != false, "skill4", _skill4 ).
				MaybeAttribute( _skill5 != false, "skill5", _skill5 ).
                Line("}");		
		}
    }

    public sealed class Trigger : IUwmfEntry
    {
        private bool _xHasBeenSet = false;
        private int _x;
        private bool _yHasBeenSet = false;
        private int _y;
        private bool _zHasBeenSet = false;
        private int _z;
        private bool _actionHasBeenSet = false;
        private int _action;
        private bool _arg0HasBeenSet = false;
        private int _arg0 = 0;
        private bool _arg1HasBeenSet = false;
        private int _arg1 = 0;
        private bool _arg2HasBeenSet = false;
        private int _arg2 = 0;
        private bool _arg3HasBeenSet = false;
        private int _arg3 = 0;
        private bool _arg4HasBeenSet = false;
        private int _arg4 = 0;
        private bool _activateEastHasBeenSet = false;
        private bool _activateEast = true;
        private bool _activateNorthHasBeenSet = false;
        private bool _activateNorth = true;
        private bool _activateWestHasBeenSet = false;
        private bool _activateWest = true;
        private bool _activateSouthHasBeenSet = false;
        private bool _activateSouth = true;
        private bool _playerCrossHasBeenSet = false;
        private bool _playerCross = false;
        private bool _playerUseHasBeenSet = false;
        private bool _playerUse = false;
        private bool _monsterUseHasBeenSet = false;
        private bool _monsterUse = false;
        private bool _repeatableHasBeenSet = false;
        private bool _repeatable = false;
        private bool _secretHasBeenSet = false;
        private bool _secret = false;

        public int X
        {
            get { return _x; }
            set 
			{ 
				_xHasBeenSet = true;
				_x = value;
			}
        }
        public int Y
        {
            get { return _y; }
            set 
			{ 
				_yHasBeenSet = true;
				_y = value;
			}
        }
        public int Z
        {
            get { return _z; }
            set 
			{ 
				_zHasBeenSet = true;
				_z = value;
			}
        }
        public int Action
        {
            get { return _action; }
            set 
			{ 
				_actionHasBeenSet = true;
				_action = value;
			}
        }
        public int Arg0
        {
            get { return _arg0; }
            set 
			{ 
				_arg0HasBeenSet = true;
				_arg0 = value;
			}
        }
        public int Arg1
        {
            get { return _arg1; }
            set 
			{ 
				_arg1HasBeenSet = true;
				_arg1 = value;
			}
        }
        public int Arg2
        {
            get { return _arg2; }
            set 
			{ 
				_arg2HasBeenSet = true;
				_arg2 = value;
			}
        }
        public int Arg3
        {
            get { return _arg3; }
            set 
			{ 
				_arg3HasBeenSet = true;
				_arg3 = value;
			}
        }
        public int Arg4
        {
            get { return _arg4; }
            set 
			{ 
				_arg4HasBeenSet = true;
				_arg4 = value;
			}
        }
        public bool ActivateEast
        {
            get { return _activateEast; }
            set 
			{ 
				_activateEastHasBeenSet = true;
				_activateEast = value;
			}
        }
        public bool ActivateNorth
        {
            get { return _activateNorth; }
            set 
			{ 
				_activateNorthHasBeenSet = true;
				_activateNorth = value;
			}
        }
        public bool ActivateWest
        {
            get { return _activateWest; }
            set 
			{ 
				_activateWestHasBeenSet = true;
				_activateWest = value;
			}
        }
        public bool ActivateSouth
        {
            get { return _activateSouth; }
            set 
			{ 
				_activateSouthHasBeenSet = true;
				_activateSouth = value;
			}
        }
        public bool PlayerCross
        {
            get { return _playerCross; }
            set 
			{ 
				_playerCrossHasBeenSet = true;
				_playerCross = value;
			}
        }
        public bool PlayerUse
        {
            get { return _playerUse; }
            set 
			{ 
				_playerUseHasBeenSet = true;
				_playerUse = value;
			}
        }
        public bool MonsterUse
        {
            get { return _monsterUse; }
            set 
			{ 
				_monsterUseHasBeenSet = true;
				_monsterUse = value;
			}
        }
        public bool Repeatable
        {
            get { return _repeatable; }
            set 
			{ 
				_repeatableHasBeenSet = true;
				_repeatable = value;
			}
        }
        public bool Secret
        {
            get { return _secret; }
            set 
			{ 
				_secretHasBeenSet = true;
				_secret = value;
			}
        }

		public Stream WriteTo(Stream stream)
        {
			if( ! _xHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set X on Trigger");
			}
			if( ! _yHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Y on Trigger");
			}
			if( ! _zHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Z on Trigger");
			}
			if( ! _actionHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Action on Trigger");
			}

            return stream.
                Line("trigger").
                Line("{").
				Attribute( "x", _x ).
				Attribute( "y", _y ).
				Attribute( "z", _z ).
				Attribute( "action", _action ).
				MaybeAttribute( _arg0 != 0, "arg0", _arg0 ).
				MaybeAttribute( _arg1 != 0, "arg1", _arg1 ).
				MaybeAttribute( _arg2 != 0, "arg2", _arg2 ).
				MaybeAttribute( _arg3 != 0, "arg3", _arg3 ).
				MaybeAttribute( _arg4 != 0, "arg4", _arg4 ).
				MaybeAttribute( _activateEast != true, "activateEast", _activateEast ).
				MaybeAttribute( _activateNorth != true, "activateNorth", _activateNorth ).
				MaybeAttribute( _activateWest != true, "activateWest", _activateWest ).
				MaybeAttribute( _activateSouth != true, "activateSouth", _activateSouth ).
				MaybeAttribute( _playerCross != false, "playerCross", _playerCross ).
				MaybeAttribute( _playerUse != false, "playerUse", _playerUse ).
				MaybeAttribute( _monsterUse != false, "monsterUse", _monsterUse ).
				MaybeAttribute( _repeatable != false, "repeatable", _repeatable ).
				MaybeAttribute( _secret != false, "secret", _secret ).
                Line("}");		
		}
    }

}