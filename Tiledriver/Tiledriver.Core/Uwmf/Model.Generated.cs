/*
** Model.Generated.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

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
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

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
            WriteProperty( stream, "textureEast", _textureEast, indent: true );
            WriteProperty( stream, "textureNorth", _textureNorth, indent: true );
            WriteProperty( stream, "textureWest", _textureWest, indent: true );
            WriteProperty( stream, "textureSouth", _textureSouth, indent: true );
            if( BlockingEast != true )
            {
                WriteProperty( stream, "blockingEast", BlockingEast, indent: true );
            }
            if( BlockingNorth != true )
            {
                WriteProperty( stream, "blockingNorth", BlockingNorth, indent: true );
            }
            if( BlockingWest != true )
            {
                WriteProperty( stream, "blockingWest", BlockingWest, indent: true );
            }
            if( BlockingSouth != true )
            {
                WriteProperty( stream, "blockingSouth", BlockingSouth, indent: true );
            }
            if( OffsetVertical != false )
            {
                WriteProperty( stream, "offsetVertical", OffsetVertical, indent: true );
            }
            if( OffsetHorizontal != false )
            {
                WriteProperty( stream, "offsetHorizontal", OffsetHorizontal, indent: true );
            }
            if( DontOverlay != false )
            {
                WriteProperty( stream, "dontOverlay", DontOverlay, indent: true );
            }
            if( Mapped != 0 )
            {
                WriteProperty( stream, "mapped", Mapped, indent: true );
            }
            if( SoundSequence != "" )
            {
                WriteProperty( stream, "soundSequence", SoundSequence, indent: true );
            }
            if( TextureOverhead != "" )
            {
                WriteProperty( stream, "textureOverhead", TextureOverhead, indent: true );
            }
            if( Comment != "" )
            {
                WriteProperty( stream, "comment", Comment, indent: true );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: true );
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
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

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
            WriteProperty( stream, "textureCeiling", _textureCeiling, indent: true );
            WriteProperty( stream, "textureFloor", _textureFloor, indent: true );
            if( Comment != "" )
            {
                WriteProperty( stream, "comment", Comment, indent: true );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: true );
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
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

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
                WriteProperty( stream, "comment", Comment, indent: true );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: true );
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
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

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
            WriteProperty( stream, "depth", _depth, indent: true );
            if( Comment != "" )
            {
                WriteProperty( stream, "comment", Comment, indent: true );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: true );
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
        private string _type;
        public string Type
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
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

        public Thing() { }

        public Thing(
            string type,
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
            WriteProperty( stream, "type", _type, indent: true );
            WriteProperty( stream, "x", _x, indent: true );
            WriteProperty( stream, "y", _y, indent: true );
            WriteProperty( stream, "z", _z, indent: true );
            WriteProperty( stream, "angle", _angle, indent: true );
            if( Ambush != false )
            {
                WriteProperty( stream, "ambush", Ambush, indent: true );
            }
            if( Patrol != false )
            {
                WriteProperty( stream, "patrol", Patrol, indent: true );
            }
            if( Skill1 != false )
            {
                WriteProperty( stream, "skill1", Skill1, indent: true );
            }
            if( Skill2 != false )
            {
                WriteProperty( stream, "skill2", Skill2, indent: true );
            }
            if( Skill3 != false )
            {
                WriteProperty( stream, "skill3", Skill3, indent: true );
            }
            if( Skill4 != false )
            {
                WriteProperty( stream, "skill4", Skill4, indent: true );
            }
            if( Skill5 != false )
            {
                WriteProperty( stream, "skill5", Skill5, indent: true );
            }
            if( Comment != "" )
            {
                WriteProperty( stream, "comment", Comment, indent: true );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: true );
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
        private string _action;
        public string Action
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
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

        public Trigger() { }

        public Trigger(
            int x,
            int y,
            int z,
            string action,
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
            WriteProperty( stream, "x", _x, indent: true );
            WriteProperty( stream, "y", _y, indent: true );
            WriteProperty( stream, "z", _z, indent: true );
            WriteProperty( stream, "action", _action, indent: true );
            if( Arg0 != 0 )
            {
                WriteProperty( stream, "arg0", Arg0, indent: true );
            }
            if( Arg1 != 0 )
            {
                WriteProperty( stream, "arg1", Arg1, indent: true );
            }
            if( Arg2 != 0 )
            {
                WriteProperty( stream, "arg2", Arg2, indent: true );
            }
            if( Arg3 != 0 )
            {
                WriteProperty( stream, "arg3", Arg3, indent: true );
            }
            if( Arg4 != 0 )
            {
                WriteProperty( stream, "arg4", Arg4, indent: true );
            }
            if( ActivateEast != true )
            {
                WriteProperty( stream, "activateEast", ActivateEast, indent: true );
            }
            if( ActivateNorth != true )
            {
                WriteProperty( stream, "activateNorth", ActivateNorth, indent: true );
            }
            if( ActivateWest != true )
            {
                WriteProperty( stream, "activateWest", ActivateWest, indent: true );
            }
            if( ActivateSouth != true )
            {
                WriteProperty( stream, "activateSouth", ActivateSouth, indent: true );
            }
            if( PlayerCross != false )
            {
                WriteProperty( stream, "playerCross", PlayerCross, indent: true );
            }
            if( PlayerUse != false )
            {
                WriteProperty( stream, "playerUse", PlayerUse, indent: true );
            }
            if( MonsterUse != false )
            {
                WriteProperty( stream, "monsterUse", MonsterUse, indent: true );
            }
            if( Repeatable != false )
            {
                WriteProperty( stream, "repeatable", Repeatable, indent: true );
            }
            if( Secret != false )
            {
                WriteProperty( stream, "secret", Secret, indent: true );
            }
            if( Comment != "" )
            {
                WriteProperty( stream, "comment", Comment, indent: true );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: true );
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
        public string Comment { get; set; } = "";
        public readonly List<Tile> Tiles = new List<Tile>();
        public readonly List<Sector> Sectors = new List<Sector>();
        public readonly List<Zone> Zones = new List<Zone>();
        public readonly List<Plane> Planes = new List<Plane>();
        public readonly List<PlaneMap> PlaneMaps = new List<PlaneMap>();
        public readonly List<Thing> Things = new List<Thing>();
        public readonly List<Trigger> Triggers = new List<Trigger>();
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public List<UnknownBlock> UnknownBlocks { get; } = new List<UnknownBlock>();

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
            IEnumerable<Trigger> triggers,
            string comment = "")
        {
            Namespace = nameSpace;
            TileSize = tileSize;
            Name = name;
            Width = width;
            Height = height;
            Comment = comment;
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

            WriteProperty( stream, "namespace", _namespace, indent: false );
            WriteProperty( stream, "tileSize", _tileSize, indent: false );
            WriteProperty( stream, "name", _name, indent: false );
            WriteProperty( stream, "width", _width, indent: false );
            WriteProperty( stream, "height", _height, indent: false );
            if( Comment != "" )
            {
                WriteProperty( stream, "comment", Comment, indent: false );
            }
            foreach( var property in UnknownProperties )
            {
                WritePropertyVerbatim( stream, (string)property.Name, property.Value, indent: false );
            }
            WriteBlocks( stream,  Tiles );
            WriteBlocks( stream,  Sectors );
            WriteBlocks( stream,  Zones );
            WriteBlocks( stream,  Planes );
            WriteBlocks( stream,  PlaneMaps );
            WriteBlocks( stream,  Things );
            WriteBlocks( stream,  Triggers );
            WriteBlocks( stream,  UnknownBlocks );
                
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