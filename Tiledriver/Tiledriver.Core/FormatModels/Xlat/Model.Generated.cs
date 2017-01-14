// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.Extensions;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class AmbushModzone
    {
        public bool Fillzone { get; set; } = false;
        public AmbushModzone() { }
        public AmbushModzone(
            bool fillzone = false)
        {
            Fillzone = fillzone;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ChangeTriggerModzone
    {
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
        private bool _positionlessTriggerHasBeenSet = false;
        private PositionlessTrigger _positionlessTrigger;
        public PositionlessTrigger PositionlessTrigger
        {
            get { return _positionlessTrigger; }
            set
            {
                _positionlessTriggerHasBeenSet = true;
                _positionlessTrigger = value;
            }
        }
        public bool Fillzone { get; set; } = false;
        public ChangeTriggerModzone() { }
        public ChangeTriggerModzone(
            string action,
            PositionlessTrigger positionlessTrigger,
            bool fillzone = false)
        {
            Action = action;
            PositionlessTrigger = positionlessTrigger;
            Fillzone = fillzone;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_actionHasBeenSet) throw new InvalidUwmfException("Did not set Action on ChangeTriggerModzone");
            if (!_positionlessTriggerHasBeenSet) throw new InvalidUwmfException("Did not set PositionlessTrigger on ChangeTriggerModzone");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ThingDefinition
    {
        private bool _actorHasBeenSet = false;
        private string _actor;
        public string Actor
        {
            get { return _actor; }
            set
            {
                _actorHasBeenSet = true;
                _actor = value;
            }
        }
        private bool _anglesHasBeenSet = false;
        private int _angles;
        public int Angles
        {
            get { return _angles; }
            set
            {
                _anglesHasBeenSet = true;
                _angles = value;
            }
        }
        private bool _holowallHasBeenSet = false;
        private bool _holowall;
        public bool Holowall
        {
            get { return _holowall; }
            set
            {
                _holowallHasBeenSet = true;
                _holowall = value;
            }
        }
        private bool _pathingHasBeenSet = false;
        private bool _pathing;
        public bool Pathing
        {
            get { return _pathing; }
            set
            {
                _pathingHasBeenSet = true;
                _pathing = value;
            }
        }
        private bool _ambushHasBeenSet = false;
        private bool _ambush;
        public bool Ambush
        {
            get { return _ambush; }
            set
            {
                _ambushHasBeenSet = true;
                _ambush = value;
            }
        }
        private bool _minskillHasBeenSet = false;
        private int _minskill;
        public int Minskill
        {
            get { return _minskill; }
            set
            {
                _minskillHasBeenSet = true;
                _minskill = value;
            }
        }
        public ThingDefinition() { }
        public ThingDefinition(
            string actor,
            int angles,
            bool holowall,
            bool pathing,
            bool ambush,
            int minskill)
        {
            Actor = actor;
            Angles = angles;
            Holowall = holowall;
            Pathing = pathing;
            Ambush = ambush;
            Minskill = minskill;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_actorHasBeenSet) throw new InvalidUwmfException("Did not set Actor on ThingDefinition");
            if (!_anglesHasBeenSet) throw new InvalidUwmfException("Did not set Angles on ThingDefinition");
            if (!_holowallHasBeenSet) throw new InvalidUwmfException("Did not set Holowall on ThingDefinition");
            if (!_pathingHasBeenSet) throw new InvalidUwmfException("Did not set Pathing on ThingDefinition");
            if (!_ambushHasBeenSet) throw new InvalidUwmfException("Did not set Ambush on ThingDefinition");
            if (!_minskillHasBeenSet) throw new InvalidUwmfException("Did not set Minskill on ThingDefinition");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class PositionlessTrigger
    {
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
        public PositionlessTrigger() { }
        public PositionlessTrigger(
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
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
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
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_actionHasBeenSet) throw new InvalidUwmfException("Did not set Action on PositionlessTrigger");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class TileMappings
    {
        public Dictionary<ushort,AmbushModzone> AmbushModzones { get; } = new Dictionary<ushort,AmbushModzone>();
        public Dictionary<ushort,ChangeTriggerModzone> ChangeTriggerModzones { get; } = new Dictionary<ushort,ChangeTriggerModzone>();
        public Dictionary<ushort,Tile> Tiles { get; } = new Dictionary<ushort,Tile>();
        public Dictionary<ushort,PositionlessTrigger> PositionlessTriggers { get; } = new Dictionary<ushort,PositionlessTrigger>();
        public Dictionary<ushort,Zone> Zones { get; } = new Dictionary<ushort,Zone>();
        public TileMappings() { }
        public TileMappings(
            Dictionary<ushort,AmbushModzone> ambushModzones,
            Dictionary<ushort,ChangeTriggerModzone> changeTriggerModzones,
            Dictionary<ushort,Tile> tiles,
            Dictionary<ushort,PositionlessTrigger> positionlessTriggers,
            Dictionary<ushort,Zone> zones)
        {
            AmbushModzones.AddRange(ambushModzones);
            ChangeTriggerModzones.AddRange(changeTriggerModzones);
            Tiles.AddRange(tiles);
            PositionlessTriggers.AddRange(positionlessTriggers);
            Zones.AddRange(zones);
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ThingMappings
    {
        public HashSet<ushort> Elevators { get; } = new HashSet<ushort>();
        public Dictionary<ushort,PositionlessTrigger> PositionlessTriggers { get; } = new Dictionary<ushort,PositionlessTrigger>();
        public Dictionary<ushort,ThingDefinition> ThingDefinitions { get; } = new Dictionary<ushort,ThingDefinition>();
        public ThingMappings() { }
        public ThingMappings(
            IEnumerable<ushort> elevators,
            Dictionary<ushort,PositionlessTrigger> positionlessTriggers,
            Dictionary<ushort,ThingDefinition> thingDefinitions)
        {
            Elevators.AddRange(elevators);
            PositionlessTriggers.AddRange(positionlessTriggers);
            ThingDefinitions.AddRange(thingDefinitions);
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class FlatMappings
    {
        public List<string> Ceilings { get; } = new List<string>();
        public List<string> Floors { get; } = new List<string>();
        public FlatMappings() { }
        public FlatMappings(
            IEnumerable<string> ceilings,
            IEnumerable<string> floors)
        {
            Ceilings.AddRange(ceilings);
            Floors.AddRange(floors);
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

}
