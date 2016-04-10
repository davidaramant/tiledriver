﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf
{
    public sealed partial class Tile : BaseUwmfBlock, IWriteableUwmfBlock 
    {
        private bool _textureEastHasBeenSet = false;
        private string _textureEast;
        public string TextureEast
        {
            get { return _textureEast; }
            set 
            { 
                _textureEastHasBeenSet = true;
                _textureEast = value;
            }
        }
        private bool _textureNorthHasBeenSet = false;
        private string _textureNorth;
        public string TextureNorth
        {
            get { return _textureNorth; }
            set 
            { 
                _textureNorthHasBeenSet = true;
                _textureNorth = value;
            }
        }
        private bool _textureWestHasBeenSet = false;
        private string _textureWest;
        public string TextureWest
        {
            get { return _textureWest; }
            set 
            { 
                _textureWestHasBeenSet = true;
                _textureWest = value;
            }
        }
        private bool _textureSouthHasBeenSet = false;
        private string _textureSouth;
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
        public string Comment { get; set; } = "";

        public Tile() { }

        public Tile(
            string textureEast,
            string textureNorth,
            string textureWest,
            string textureSouth,
            bool blockingEast = true,
            bool blockingNorth = true,
            bool blockingWest = true,
            bool blockingSouth = true,
            bool offsetVertical = false,
            bool offsetHorizontal = false,
            bool dontOverlay = false,
            int mapped = 0,
            string soundSequence = "",
            string textureOverhead = "",
            string comment = "")
        {
            TextureEast = textureEast;
            TextureNorth = textureNorth;
            TextureWest = textureWest;
            TextureSouth = textureSouth;
            BlockingEast = blockingEast;
            BlockingNorth = blockingNorth;
            BlockingWest = blockingWest;
            BlockingSouth = blockingSouth;
            OffsetVertical = offsetVertical;
            OffsetHorizontal = offsetHorizontal;
            DontOverlay = dontOverlay;
            Mapped = mapped;
            SoundSequence = soundSequence;
            TextureOverhead = textureOverhead;
            Comment = comment;

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "tile");
            WriteLine( stream, "{");
            WriteAttribute( stream, "textureEast", _textureEast, indent: true );
            WriteAttribute( stream, "textureNorth", _textureNorth, indent: true );
            WriteAttribute( stream, "textureWest", _textureWest, indent: true );
            WriteAttribute( stream, "textureSouth", _textureSouth, indent: true );
            if( BlockingEast != true )
            {
                WriteAttribute( stream, "blockingEast", BlockingEast, indent: true );
            }
            if( BlockingNorth != true )
            {
                WriteAttribute( stream, "blockingNorth", BlockingNorth, indent: true );
            }
            if( BlockingWest != true )
            {
                WriteAttribute( stream, "blockingWest", BlockingWest, indent: true );
            }
            if( BlockingSouth != true )
            {
                WriteAttribute( stream, "blockingSouth", BlockingSouth, indent: true );
            }
            if( OffsetVertical != false )
            {
                WriteAttribute( stream, "offsetVertical", OffsetVertical, indent: true );
            }
            if( OffsetHorizontal != false )
            {
                WriteAttribute( stream, "offsetHorizontal", OffsetHorizontal, indent: true );
            }
            if( DontOverlay != false )
            {
                WriteAttribute( stream, "dontOverlay", DontOverlay, indent: true );
            }
            if( Mapped != 0 )
            {
                WriteAttribute( stream, "mapped", Mapped, indent: true );
            }
            if( SoundSequence != "" )
            {
                WriteAttribute( stream, "soundSequence", SoundSequence, indent: true );
            }
            if( TextureOverhead != "" )
            {
                WriteAttribute( stream, "textureOverhead", TextureOverhead, indent: true );
            }
            if( Comment != "" )
            {
                WriteAttribute( stream, "comment", Comment, indent: true );
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
        public string TextureCeiling
        {
            get { return _textureCeiling; }
            set 
            { 
                _textureCeilingHasBeenSet = true;
                _textureCeiling = value;
            }
        }
        private bool _textureFloorHasBeenSet = false;
        private string _textureFloor;
        public string TextureFloor
        {
            get { return _textureFloor; }
            set 
            { 
                _textureFloorHasBeenSet = true;
                _textureFloor = value;
            }
        }
        public string Comment { get; set; } = "";

        public Sector() { }

        public Sector(
            string textureCeiling,
            string textureFloor,
            string comment = "")
        {
            TextureCeiling = textureCeiling;
            TextureFloor = textureFloor;
            Comment = comment;

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "sector");
            WriteLine( stream, "{");
            WriteAttribute( stream, "textureCeiling", _textureCeiling, indent: true );
            WriteAttribute( stream, "textureFloor", _textureFloor, indent: true );
            if( Comment != "" )
            {
                WriteAttribute( stream, "comment", Comment, indent: true );
            }
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

        public string Comment { get; set; } = "";

        public Zone() { }

        public Zone(
            string comment = "")
        {
            Comment = comment;

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "zone");
            WriteLine( stream, "{");
            if( Comment != "" )
            {
                WriteAttribute( stream, "comment", Comment, indent: true );
            }
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
        public string Comment { get; set; } = "";

        public Plane() { }

        public Plane(
            int depth,
            string comment = "")
        {
            Depth = depth;
            Comment = comment;

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "plane");
            WriteLine( stream, "{");
            WriteAttribute( stream, "depth", _depth, indent: true );
            if( Comment != "" )
            {
                WriteAttribute( stream, "comment", Comment, indent: true );
            }
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
        public int Tile
        {
            get { return _tile; }
            set 
            { 
                _tileHasBeenSet = true;
                _tile = value;
            }
        }
        private bool _sectorHasBeenSet = false;
        private int _sector;
        public int Sector
        {
            get { return _sector; }
            set 
            { 
                _sectorHasBeenSet = true;
                _sector = value;
            }
        }
        private bool _zoneHasBeenSet = false;
        private int _zone;
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

        public TileSpace() { }

        public TileSpace(
            int tile,
            int sector,
            int zone,
            int tag = 0)
        {
            Tile = tile;
            Sector = sector;
            Zone = zone;
            Tag = tag;

            AdditionalSemanticChecks();
        }

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

        public PlaneMap() { }

        public PlaneMap(
            IEnumerable<TileSpace> tileSpaces)
        {
            TileSpaces.AddRange( tileSpaces );

            AdditionalSemanticChecks();
        }

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
        public int Type
        {
            get { return _type; }
            set 
            { 
                _typeHasBeenSet = true;
                _type = value;
            }
        }
        private bool _xHasBeenSet = false;
        private double _x;
        public double X
        {
            get { return _x; }
            set 
            { 
                _xHasBeenSet = true;
                _x = value;
            }
        }
        private bool _yHasBeenSet = false;
        private double _y;
        public double Y
        {
            get { return _y; }
            set 
            { 
                _yHasBeenSet = true;
                _y = value;
            }
        }
        private bool _zHasBeenSet = false;
        private double _z;
        public double Z
        {
            get { return _z; }
            set 
            { 
                _zHasBeenSet = true;
                _z = value;
            }
        }
        private bool _angleHasBeenSet = false;
        private int _angle;
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
        public string Comment { get; set; } = "";

        public Thing() { }

        public Thing(
            int type,
            double x,
            double y,
            double z,
            int angle,
            bool ambush = false,
            bool patrol = false,
            bool skill1 = false,
            bool skill2 = false,
            bool skill3 = false,
            bool skill4 = false,
            bool skill5 = false,
            string comment = "")
        {
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
            Ambush = ambush;
            Patrol = patrol;
            Skill1 = skill1;
            Skill2 = skill2;
            Skill3 = skill3;
            Skill4 = skill4;
            Skill5 = skill5;
            Comment = comment;

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "thing");
            WriteLine( stream, "{");
            WriteAttribute( stream, "type", _type, indent: true );
            WriteAttribute( stream, "x", _x, indent: true );
            WriteAttribute( stream, "y", _y, indent: true );
            WriteAttribute( stream, "z", _z, indent: true );
            WriteAttribute( stream, "angle", _angle, indent: true );
            if( Ambush != false )
            {
                WriteAttribute( stream, "ambush", Ambush, indent: true );
            }
            if( Patrol != false )
            {
                WriteAttribute( stream, "patrol", Patrol, indent: true );
            }
            if( Skill1 != false )
            {
                WriteAttribute( stream, "skill1", Skill1, indent: true );
            }
            if( Skill2 != false )
            {
                WriteAttribute( stream, "skill2", Skill2, indent: true );
            }
            if( Skill3 != false )
            {
                WriteAttribute( stream, "skill3", Skill3, indent: true );
            }
            if( Skill4 != false )
            {
                WriteAttribute( stream, "skill4", Skill4, indent: true );
            }
            if( Skill5 != false )
            {
                WriteAttribute( stream, "skill5", Skill5, indent: true );
            }
            if( Comment != "" )
            {
                WriteAttribute( stream, "comment", Comment, indent: true );
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
        public int X
        {
            get { return _x; }
            set 
            { 
                _xHasBeenSet = true;
                _x = value;
            }
        }
        private bool _yHasBeenSet = false;
        private int _y;
        public int Y
        {
            get { return _y; }
            set 
            { 
                _yHasBeenSet = true;
                _y = value;
            }
        }
        private bool _zHasBeenSet = false;
        private int _z;
        public int Z
        {
            get { return _z; }
            set 
            { 
                _zHasBeenSet = true;
                _z = value;
            }
        }
        private bool _actionHasBeenSet = false;
        private int _action;
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
        public string Comment { get; set; } = "";

        public Trigger() { }

        public Trigger(
            int x,
            int y,
            int z,
            int action,
            int arg0 = 0,
            int arg1 = 0,
            int arg2 = 0,
            int arg3 = 0,
            int arg4 = 0,
            bool activateEast = true,
            bool activateNorth = true,
            bool activateWest = true,
            bool activateSouth = true,
            bool playerCross = false,
            bool playerUse = false,
            bool monsterUse = false,
            bool repeatable = false,
            bool secret = false,
            string comment = "")
        {
            X = x;
            Y = y;
            Z = z;
            Action = action;
            Arg0 = arg0;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
            ActivateEast = activateEast;
            ActivateNorth = activateNorth;
            ActivateWest = activateWest;
            ActivateSouth = activateSouth;
            PlayerCross = playerCross;
            PlayerUse = playerUse;
            MonsterUse = monsterUse;
            Repeatable = repeatable;
            Secret = secret;
            Comment = comment;

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteLine( stream, "trigger");
            WriteLine( stream, "{");
            WriteAttribute( stream, "x", _x, indent: true );
            WriteAttribute( stream, "y", _y, indent: true );
            WriteAttribute( stream, "z", _z, indent: true );
            WriteAttribute( stream, "action", _action, indent: true );
            if( Arg0 != 0 )
            {
                WriteAttribute( stream, "arg0", Arg0, indent: true );
            }
            if( Arg1 != 0 )
            {
                WriteAttribute( stream, "arg1", Arg1, indent: true );
            }
            if( Arg2 != 0 )
            {
                WriteAttribute( stream, "arg2", Arg2, indent: true );
            }
            if( Arg3 != 0 )
            {
                WriteAttribute( stream, "arg3", Arg3, indent: true );
            }
            if( Arg4 != 0 )
            {
                WriteAttribute( stream, "arg4", Arg4, indent: true );
            }
            if( ActivateEast != true )
            {
                WriteAttribute( stream, "activateEast", ActivateEast, indent: true );
            }
            if( ActivateNorth != true )
            {
                WriteAttribute( stream, "activateNorth", ActivateNorth, indent: true );
            }
            if( ActivateWest != true )
            {
                WriteAttribute( stream, "activateWest", ActivateWest, indent: true );
            }
            if( ActivateSouth != true )
            {
                WriteAttribute( stream, "activateSouth", ActivateSouth, indent: true );
            }
            if( PlayerCross != false )
            {
                WriteAttribute( stream, "playerCross", PlayerCross, indent: true );
            }
            if( PlayerUse != false )
            {
                WriteAttribute( stream, "playerUse", PlayerUse, indent: true );
            }
            if( MonsterUse != false )
            {
                WriteAttribute( stream, "monsterUse", MonsterUse, indent: true );
            }
            if( Repeatable != false )
            {
                WriteAttribute( stream, "repeatable", Repeatable, indent: true );
            }
            if( Secret != false )
            {
                WriteAttribute( stream, "secret", Secret, indent: true );
            }
            if( Comment != "" )
            {
                WriteAttribute( stream, "comment", Comment, indent: true );
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
        public string Namespace
        {
            get { return _namespace; }
            set 
            { 
                _namespaceHasBeenSet = true;
                _namespace = value;
            }
        }
        private bool _tileSizeHasBeenSet = false;
        private int _tileSize;
        public int TileSize
        {
            get { return _tileSize; }
            set 
            { 
                _tileSizeHasBeenSet = true;
                _tileSize = value;
            }
        }
        private bool _nameHasBeenSet = false;
        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                _nameHasBeenSet = true;
                _name = value;
            }
        }
        private bool _widthHasBeenSet = false;
        private int _width;
        public int Width
        {
            get { return _width; }
            set 
            { 
                _widthHasBeenSet = true;
                _width = value;
            }
        }
        private bool _heightHasBeenSet = false;
        private int _height;
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

        public Map() { }

        public Map(
            string nameSpace,
            int tileSize,
            string name,
            int width,
            int height,
            IEnumerable<Tile> tiles,
            IEnumerable<Sector> sectors,
            IEnumerable<Zone> zones,
            IEnumerable<Plane> planes,
            IEnumerable<PlaneMap> planeMaps,
            IEnumerable<Thing> things,
            IEnumerable<Trigger> triggers)
        {
            Namespace = nameSpace;
            TileSize = tileSize;
            Name = name;
            Width = width;
            Height = height;
            Tiles.AddRange( tiles );
            Sectors.AddRange( sectors );
            Zones.AddRange( zones );
            Planes.AddRange( planes );
            PlaneMaps.AddRange( planeMaps );
            Things.AddRange( things );
            Triggers.AddRange( triggers );

            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            WriteAttribute( stream, "namespace", _namespace, indent: false );
            WriteAttribute( stream, "tileSize", _tileSize, indent: false );
            WriteAttribute( stream, "name", _name, indent: false );
            WriteAttribute( stream, "width", _width, indent: false );
            WriteAttribute( stream, "height", _height, indent: false );
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