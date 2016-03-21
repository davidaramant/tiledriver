// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf
{
    public sealed partial class Tile : BaseUwmfBlock, IWriteableUwmfBlock 
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
        public bool BlockingEast { get; set; } = true;
        public bool BlockingNorth { get; set; } = true;
        public bool BlockingWest { get; set; } = true;
        public bool BlockingSouth { get; set; } = true;
        public bool OffsetVertical { get; set; } = false;
        public bool OffsetHorizontal { get; set; } = false;
        public bool DontOverlay { get; set; } = false;
        public int Mapped { get; set; } = 0;
        public string SoundSequence { get; set; } = "";
        public string TextureOverhead { get; set; } = "";

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "tile");
            WriteLine( stream, "{");
            WriteAttribute( stream,  "textureEast", _textureEast );
            WriteAttribute( stream,  "textureNorth", _textureNorth );
            WriteAttribute( stream,  "textureWest", _textureWest );
            WriteAttribute( stream,  "textureSouth", _textureSouth );
            if( BlockingEast != true )
            {
                WriteAttribute( stream, "blockingEast", BlockingEast );
            }
            if( BlockingNorth != true )
            {
                WriteAttribute( stream, "blockingNorth", BlockingNorth );
            }
            if( BlockingWest != true )
            {
                WriteAttribute( stream, "blockingWest", BlockingWest );
            }
            if( BlockingSouth != true )
            {
                WriteAttribute( stream, "blockingSouth", BlockingSouth );
            }
            if( OffsetVertical != false )
            {
                WriteAttribute( stream, "offsetVertical", OffsetVertical );
            }
            if( OffsetHorizontal != false )
            {
                WriteAttribute( stream, "offsetHorizontal", OffsetHorizontal );
            }
            if( DontOverlay != false )
            {
                WriteAttribute( stream, "dontOverlay", DontOverlay );
            }
            if( Mapped != 0 )
            {
                WriteAttribute( stream, "mapped", Mapped );
            }
            if( SoundSequence != "" )
            {
                WriteAttribute( stream, "soundSequence", SoundSequence );
            }
            if( TextureOverhead != "" )
            {
                WriteAttribute( stream, "textureOverhead", TextureOverhead );
            }
            WriteLine( stream, "}");
                
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

    public sealed partial class Sector : BaseUwmfBlock, IWriteableUwmfBlock 
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

            WriteLine( stream, "sector");
            WriteLine( stream, "{");
            WriteAttribute( stream,  "textureCeiling", _textureCeiling );
            WriteAttribute( stream,  "textureFloor", _textureFloor );
            WriteLine( stream, "}");
                
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

    public sealed partial class Zone : BaseUwmfBlock, IWriteableUwmfBlock 
    {


        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "zone");
            WriteLine( stream, "{");
            WriteLine( stream, "}");
                
            return stream;
        }

        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class Plane : BaseUwmfBlock, IWriteableUwmfBlock 
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

            WriteLine( stream, "plane");
            WriteLine( stream, "{");
            WriteAttribute( stream,  "depth", _depth );
            WriteLine( stream, "}");
                
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

    public sealed partial class TileSpace : BaseUwmfBlock 
    {
        private bool _tileHasBeenSet = false;
        private int _tile;
        private bool _sectorHasBeenSet = false;
        private int _sector;
        private bool _zoneHasBeenSet = false;
        private int _zone;

        public int Tile
        {
            get { return _tile; }
            set 
            { 
                _tileHasBeenSet = true;
                _tile = value;
            }
        }
        public int Sector
        {
            get { return _sector; }
            set 
            { 
                _sectorHasBeenSet = true;
                _sector = value;
            }
        }
        public int Zone
        {
            get { return _zone; }
            set 
            { 
                _zoneHasBeenSet = true;
                _zone = value;
            }
        }
        public int Tag { get; set; } = 0;

        public void CheckSemanticValidity()
        {
            if( ! _tileHasBeenSet )
            {
                throw new InvalidUwmfException("Did not set Tile on TileSpace");
            }
            if( ! _sectorHasBeenSet )
            {
                throw new InvalidUwmfException("Did not set Sector on TileSpace");
            }
            if( ! _zoneHasBeenSet )
            {
                throw new InvalidUwmfException("Did not set Zone on TileSpace");
            }
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class PlaneMap : BaseUwmfBlock, IWriteableUwmfBlock 
    {

        public readonly List<TileSpace> TileSpaces = new List<TileSpace>();

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "planeMap");
            WriteLine( stream, "{");
            WriteBlocks( stream,  TileSpaces );
            WriteLine( stream, "}");
                
            return stream;
        }

        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class Thing : BaseUwmfBlock, IWriteableUwmfBlock 
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
        public bool Ambush { get; set; } = false;
        public bool Patrol { get; set; } = false;
        public bool Skill1 { get; set; } = false;
        public bool Skill2 { get; set; } = false;
        public bool Skill3 { get; set; } = false;
        public bool Skill4 { get; set; } = false;
        public bool Skill5 { get; set; } = false;

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "thing");
            WriteLine( stream, "{");
            WriteAttribute( stream,  "type", _type );
            WriteAttribute( stream,  "x", _x );
            WriteAttribute( stream,  "y", _y );
            WriteAttribute( stream,  "z", _z );
            WriteAttribute( stream,  "angle", _angle );
            if( Ambush != false )
            {
                WriteAttribute( stream, "ambush", Ambush );
            }
            if( Patrol != false )
            {
                WriteAttribute( stream, "patrol", Patrol );
            }
            if( Skill1 != false )
            {
                WriteAttribute( stream, "skill1", Skill1 );
            }
            if( Skill2 != false )
            {
                WriteAttribute( stream, "skill2", Skill2 );
            }
            if( Skill3 != false )
            {
                WriteAttribute( stream, "skill3", Skill3 );
            }
            if( Skill4 != false )
            {
                WriteAttribute( stream, "skill4", Skill4 );
            }
            if( Skill5 != false )
            {
                WriteAttribute( stream, "skill5", Skill5 );
            }
            WriteLine( stream, "}");
                
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

    public sealed partial class Trigger : BaseUwmfBlock, IWriteableUwmfBlock 
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
        public int Arg0 { get; set; } = 0;
        public int Arg1 { get; set; } = 0;
        public int Arg2 { get; set; } = 0;
        public int Arg3 { get; set; } = 0;
        public int Arg4 { get; set; } = 0;
        public bool ActivateEast { get; set; } = true;
        public bool ActivateNorth { get; set; } = true;
        public bool ActivateWest { get; set; } = true;
        public bool ActivateSouth { get; set; } = true;
        public bool PlayerCross { get; set; } = false;
        public bool PlayerUse { get; set; } = false;
        public bool MonsterUse { get; set; } = false;
        public bool Repeatable { get; set; } = false;
        public bool Secret { get; set; } = false;

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "trigger");
            WriteLine( stream, "{");
            WriteAttribute( stream,  "x", _x );
            WriteAttribute( stream,  "y", _y );
            WriteAttribute( stream,  "z", _z );
            WriteAttribute( stream,  "action", _action );
            if( Arg0 != 0 )
            {
                WriteAttribute( stream, "arg0", Arg0 );
            }
            if( Arg1 != 0 )
            {
                WriteAttribute( stream, "arg1", Arg1 );
            }
            if( Arg2 != 0 )
            {
                WriteAttribute( stream, "arg2", Arg2 );
            }
            if( Arg3 != 0 )
            {
                WriteAttribute( stream, "arg3", Arg3 );
            }
            if( Arg4 != 0 )
            {
                WriteAttribute( stream, "arg4", Arg4 );
            }
            if( ActivateEast != true )
            {
                WriteAttribute( stream, "activateEast", ActivateEast );
            }
            if( ActivateNorth != true )
            {
                WriteAttribute( stream, "activateNorth", ActivateNorth );
            }
            if( ActivateWest != true )
            {
                WriteAttribute( stream, "activateWest", ActivateWest );
            }
            if( ActivateSouth != true )
            {
                WriteAttribute( stream, "activateSouth", ActivateSouth );
            }
            if( PlayerCross != false )
            {
                WriteAttribute( stream, "playerCross", PlayerCross );
            }
            if( PlayerUse != false )
            {
                WriteAttribute( stream, "playerUse", PlayerUse );
            }
            if( MonsterUse != false )
            {
                WriteAttribute( stream, "monsterUse", MonsterUse );
            }
            if( Repeatable != false )
            {
                WriteAttribute( stream, "repeatable", Repeatable );
            }
            if( Secret != false )
            {
                WriteAttribute( stream, "secret", Secret );
            }
            WriteLine( stream, "}");
                
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

    public sealed partial class Map : BaseUwmfBlock, IWriteableUwmfBlock 
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
        public readonly List<PlaneMap> PlaneMaps = new List<PlaneMap>();
        public readonly List<Thing> Things = new List<Thing>();
        public readonly List<Trigger> Triggers = new List<Trigger>();

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteAttribute( stream,  "namespace", _namespace );
            WriteAttribute( stream,  "tileSize", _tileSize );
            WriteAttribute( stream,  "name", _name );
            WriteAttribute( stream,  "width", _width );
            WriteAttribute( stream,  "height", _height );
            WriteBlocks( stream,  Tiles );
            WriteBlocks( stream,  Sectors );
            WriteBlocks( stream,  Zones );
            WriteBlocks( stream,  Planes );
            WriteBlocks( stream,  PlaneMaps );
            WriteBlocks( stream,  Things );
            WriteBlocks( stream,  Triggers );
                
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