// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Core.Uwmf
{
    public sealed partial class Tile : IUwmfEntry
    {
        private bool _textureEastHasBeenSet = false;
        private string _textureEast;
        private bool _textureNorthHasBeenSet = false;
        private string _textureNorth;
        private bool _textureWestHasBeenSet = false;
        private string _textureWest;
        private bool _textureSouthHasBeenSet = false;
        private string _textureSouth;

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
        public bool BlockingEast {get; set; }
        public bool BlockingNorth {get; set; }
        public bool BlockingWest {get; set; }
        public bool BlockingSouth {get; set; }
        public bool OffsetVertical {get; set; }
        public bool OffsetHorizontal {get; set; }
        public bool DontOverlay {get; set; }
        public int Mapped {get; set; }
        public string SoundSequence {get; set; }
        public string TextureOverhead {get; set; }

		public Stream WriteTo(Stream stream)
        {
			CheckSemanticValidity();

            return stream.
                Line("tile").
                Line("{").
				Attribute( "textureEast", _textureEast ).
				Attribute( "textureNorth", _textureNorth ).
				Attribute( "textureWest", _textureWest ).
				Attribute( "textureSouth", _textureSouth ).
				MaybeAttribute( Mapped != 0, "mapped", Mapped ).
				MaybeAttribute( BlockingEast != true, "blockingEast", BlockingEast ).
				MaybeAttribute( BlockingNorth != true, "blockingNorth", BlockingNorth ).
				MaybeAttribute( BlockingWest != true, "blockingWest", BlockingWest ).
				MaybeAttribute( BlockingSouth != true, "blockingSouth", BlockingSouth ).
				MaybeAttribute( OffsetVertical != false, "offsetVertical", OffsetVertical ).
				MaybeAttribute( OffsetHorizontal != false, "offsetHorizontal", OffsetHorizontal ).
				MaybeAttribute( DontOverlay != false, "dontOverlay", DontOverlay ).
				MaybeAttribute( SoundSequence != "", "soundSequence", SoundSequence ).
				MaybeAttribute( TextureOverhead != "", "textureOverhead", TextureOverhead ).
                Line("}");		
		}

		public void CheckSemanticValidity()
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
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

    public sealed partial class Sector : IUwmfEntry
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
			CheckSemanticValidity();

            return stream.
                Line("sector").
                Line("{").
				Attribute( "textureCeiling", _textureCeiling ).
				Attribute( "textureFloor", _textureFloor ).
                Line("}");		
		}

		public void CheckSemanticValidity()
		{
			if( ! _textureCeilingHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureCeiling on Sector");
			}
			if( ! _textureFloorHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TextureFloor on Sector");
			}
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

    public sealed partial class Zone : IUwmfEntry
    {


		public Stream WriteTo(Stream stream)
        {
			CheckSemanticValidity();

            return stream.
                Line("zone").
                Line("{").
                Line("}");		
		}

		public void CheckSemanticValidity()
		{
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

    public sealed partial class Plane : IUwmfEntry
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
			CheckSemanticValidity();

            return stream.
                Line("plane").
                Line("{").
				Attribute( "depth", _depth ).
                Line("}");		
		}

		public void CheckSemanticValidity()
		{
			if( ! _depthHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Depth on Plane");
			}
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

    public sealed partial class Thing : IUwmfEntry
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
        public bool Ambush {get; set; }
        public bool Patrol {get; set; }
        public bool Skill1 {get; set; }
        public bool Skill2 {get; set; }
        public bool Skill3 {get; set; }
        public bool Skill4 {get; set; }
        public bool Skill5 {get; set; }

		public Stream WriteTo(Stream stream)
        {
			CheckSemanticValidity();

            return stream.
                Line("thing").
                Line("{").
				Attribute( "type", _type ).
				Attribute( "x", _x ).
				Attribute( "y", _y ).
				Attribute( "z", _z ).
				Attribute( "angle", _angle ).
				MaybeAttribute( Ambush != false, "ambush", Ambush ).
				MaybeAttribute( Patrol != false, "patrol", Patrol ).
				MaybeAttribute( Skill1 != false, "skill1", Skill1 ).
				MaybeAttribute( Skill2 != false, "skill2", Skill2 ).
				MaybeAttribute( Skill3 != false, "skill3", Skill3 ).
				MaybeAttribute( Skill4 != false, "skill4", Skill4 ).
				MaybeAttribute( Skill5 != false, "skill5", Skill5 ).
                Line("}");		
		}

		public void CheckSemanticValidity()
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
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

    public sealed partial class Trigger : IUwmfEntry
    {
        private bool _xHasBeenSet = false;
        private int _x;
        private bool _yHasBeenSet = false;
        private int _y;
        private bool _zHasBeenSet = false;
        private int _z;
        private bool _actionHasBeenSet = false;
        private int _action;

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
        public int Arg0 {get; set; }
        public int Arg1 {get; set; }
        public int Arg2 {get; set; }
        public int Arg3 {get; set; }
        public int Arg4 {get; set; }
        public bool ActivateEast {get; set; }
        public bool ActivateNorth {get; set; }
        public bool ActivateWest {get; set; }
        public bool ActivateSouth {get; set; }
        public bool PlayerCross {get; set; }
        public bool PlayerUse {get; set; }
        public bool MonsterUse {get; set; }
        public bool Repeatable {get; set; }
        public bool Secret {get; set; }

		public Stream WriteTo(Stream stream)
        {
			CheckSemanticValidity();

            return stream.
                Line("trigger").
                Line("{").
				Attribute( "x", _x ).
				Attribute( "y", _y ).
				Attribute( "z", _z ).
				Attribute( "action", _action ).
				MaybeAttribute( Arg0 != 0, "arg0", Arg0 ).
				MaybeAttribute( Arg1 != 0, "arg1", Arg1 ).
				MaybeAttribute( Arg2 != 0, "arg2", Arg2 ).
				MaybeAttribute( Arg3 != 0, "arg3", Arg3 ).
				MaybeAttribute( Arg4 != 0, "arg4", Arg4 ).
				MaybeAttribute( ActivateEast != true, "activateEast", ActivateEast ).
				MaybeAttribute( ActivateNorth != true, "activateNorth", ActivateNorth ).
				MaybeAttribute( ActivateWest != true, "activateWest", ActivateWest ).
				MaybeAttribute( ActivateSouth != true, "activateSouth", ActivateSouth ).
				MaybeAttribute( PlayerCross != false, "playerCross", PlayerCross ).
				MaybeAttribute( PlayerUse != false, "playerUse", PlayerUse ).
				MaybeAttribute( MonsterUse != false, "monsterUse", MonsterUse ).
				MaybeAttribute( Repeatable != false, "repeatable", Repeatable ).
				MaybeAttribute( Secret != false, "secret", Secret ).
                Line("}");		
		}

		public void CheckSemanticValidity()
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
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

}