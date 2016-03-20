// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
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

            stream.Line("tile");
            stream.Line("{");
			stream.Attribute( "textureEast", _textureEast );
			stream.Attribute( "textureNorth", _textureNorth );
			stream.Attribute( "textureWest", _textureWest );
			stream.Attribute( "textureSouth", _textureSouth );
			stream.MaybeAttribute( Mapped != 0, "mapped", Mapped );
			stream.MaybeAttribute( BlockingEast != true, "blockingEast", BlockingEast );
			stream.MaybeAttribute( BlockingNorth != true, "blockingNorth", BlockingNorth );
			stream.MaybeAttribute( BlockingWest != true, "blockingWest", BlockingWest );
			stream.MaybeAttribute( BlockingSouth != true, "blockingSouth", BlockingSouth );
			stream.MaybeAttribute( OffsetVertical != false, "offsetVertical", OffsetVertical );
			stream.MaybeAttribute( OffsetHorizontal != false, "offsetHorizontal", OffsetHorizontal );
			stream.MaybeAttribute( DontOverlay != false, "dontOverlay", DontOverlay );
			stream.MaybeAttribute( SoundSequence != "", "soundSequence", SoundSequence );
			stream.MaybeAttribute( TextureOverhead != "", "textureOverhead", TextureOverhead );
            stream.Line("}");
				
			return stream;
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

            stream.Line("sector");
            stream.Line("{");
			stream.Attribute( "textureCeiling", _textureCeiling );
			stream.Attribute( "textureFloor", _textureFloor );
            stream.Line("}");
				
			return stream;
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

            stream.Line("zone");
            stream.Line("{");
            stream.Line("}");
				
			return stream;
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

            stream.Line("plane");
            stream.Line("{");
			stream.Attribute( "depth", _depth );
            stream.Line("}");
				
			return stream;
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

            stream.Line("thing");
            stream.Line("{");
			stream.Attribute( "type", _type );
			stream.Attribute( "x", _x );
			stream.Attribute( "y", _y );
			stream.Attribute( "z", _z );
			stream.Attribute( "angle", _angle );
			stream.MaybeAttribute( Ambush != false, "ambush", Ambush );
			stream.MaybeAttribute( Patrol != false, "patrol", Patrol );
			stream.MaybeAttribute( Skill1 != false, "skill1", Skill1 );
			stream.MaybeAttribute( Skill2 != false, "skill2", Skill2 );
			stream.MaybeAttribute( Skill3 != false, "skill3", Skill3 );
			stream.MaybeAttribute( Skill4 != false, "skill4", Skill4 );
			stream.MaybeAttribute( Skill5 != false, "skill5", Skill5 );
            stream.Line("}");
				
			return stream;
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

            stream.Line("trigger");
            stream.Line("{");
			stream.Attribute( "x", _x );
			stream.Attribute( "y", _y );
			stream.Attribute( "z", _z );
			stream.Attribute( "action", _action );
			stream.MaybeAttribute( Arg0 != 0, "arg0", Arg0 );
			stream.MaybeAttribute( Arg1 != 0, "arg1", Arg1 );
			stream.MaybeAttribute( Arg2 != 0, "arg2", Arg2 );
			stream.MaybeAttribute( Arg3 != 0, "arg3", Arg3 );
			stream.MaybeAttribute( Arg4 != 0, "arg4", Arg4 );
			stream.MaybeAttribute( ActivateEast != true, "activateEast", ActivateEast );
			stream.MaybeAttribute( ActivateNorth != true, "activateNorth", ActivateNorth );
			stream.MaybeAttribute( ActivateWest != true, "activateWest", ActivateWest );
			stream.MaybeAttribute( ActivateSouth != true, "activateSouth", ActivateSouth );
			stream.MaybeAttribute( PlayerCross != false, "playerCross", PlayerCross );
			stream.MaybeAttribute( PlayerUse != false, "playerUse", PlayerUse );
			stream.MaybeAttribute( MonsterUse != false, "monsterUse", MonsterUse );
			stream.MaybeAttribute( Repeatable != false, "repeatable", Repeatable );
			stream.MaybeAttribute( Secret != false, "secret", Secret );
            stream.Line("}");
				
			return stream;
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

    public sealed partial class Map : IUwmfEntry
    {
        private bool _namespaceHasBeenSet = false;
        private string _namespace;
        private bool _tileSizeHasBeenSet = false;
        private int _tileSize;
        private bool _nameHasBeenSet = false;
        private string _name;
        private bool _widthHasBeenSet = false;
        private int _width;
        private bool _heightHasBeenSet = false;
        private int _height;

        public string Namespace
        {
            get { return _namespace; }
            set 
			{ 
				_namespaceHasBeenSet = true;
				_namespace = value;
			}
        }
        public int TileSize
        {
            get { return _tileSize; }
            set 
			{ 
				_tileSizeHasBeenSet = true;
				_tileSize = value;
			}
        }
        public string Name
        {
            get { return _name; }
            set 
			{ 
				_nameHasBeenSet = true;
				_name = value;
			}
        }
        public int Width
        {
            get { return _width; }
            set 
			{ 
				_widthHasBeenSet = true;
				_width = value;
			}
        }
        public int Height
        {
            get { return _height; }
            set 
			{ 
				_heightHasBeenSet = true;
				_height = value;
			}
        }
		public readonly List<Tile> Tiles = new List<Tile>();
		public readonly List<Sector> Sectors = new List<Sector>();
		public readonly List<Zone> Zones = new List<Zone>();
		public readonly List<Plane> Planes = new List<Plane>();
		public readonly List<Thing> Things = new List<Thing>();
		public readonly List<Trigger> Triggers = new List<Trigger>();

		public Stream WriteTo(Stream stream)
        {
			CheckSemanticValidity();

			stream.Attribute( "namespace", _namespace );
			stream.Attribute( "tileSize", _tileSize );
			stream.Attribute( "name", _name );
			stream.Attribute( "width", _width );
			stream.Attribute( "height", _height );
			stream.Blocks( Tiles );
			stream.Blocks( Sectors );
			stream.Blocks( Zones );
			stream.Blocks( Planes );
			stream.Blocks( Things );
			stream.Blocks( Triggers );
				
			return stream;
		}

		public void CheckSemanticValidity()
		{
			if( ! _namespaceHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Namespace on Map");
			}
			if( ! _tileSizeHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set TileSize on Map");
			}
			if( ! _nameHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Name on Map");
			}
			if( ! _widthHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Width on Map");
			}
			if( ! _heightHasBeenSet )
			{
				throw new InvalidUwmfException("Did not set Height on Map");
			}
			AdditionalSemanticChecks();
		}

		partial void AdditionalSemanticChecks();
    }

}