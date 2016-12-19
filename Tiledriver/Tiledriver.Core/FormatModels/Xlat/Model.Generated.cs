// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
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
        private bool _triggerHasBeenSet = false;
        private Trigger _trigger;
        public Trigger Trigger
        {
            get { return _trigger; }
            set
            {
                _triggerHasBeenSet = true;
                _trigger = value;
            }
        }
        public bool Fillzone { get; set; } = false;
        public ChangeTriggerModzone() { }
        public ChangeTriggerModzone(
            string action,
            Trigger trigger,
            bool fillzone = false)
        {
            Action = action;
            Trigger = trigger;
            Fillzone = fillzone;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_actionHasBeenSet) throw new InvalidUwmfException("Did not set Action on ChangeTriggerModzone");
            if (!_triggerHasBeenSet) throw new InvalidUwmfException("Did not set Trigger on ChangeTriggerModzone");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ThingDefinition
    {
        private bool _oldnumHasBeenSet = false;
        private ushort _oldnum;
        public ushort Oldnum
        {
            get { return _oldnum; }
            set
            {
                _oldnumHasBeenSet = true;
                _oldnum = value;
            }
        }
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
            ushort oldnum,
            string actor,
            int angles,
            bool holowall,
            bool pathing,
            int minskill)
        {
            Oldnum = oldnum;
            Actor = actor;
            Angles = angles;
            Holowall = holowall;
            Pathing = pathing;
            Minskill = minskill;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldnumHasBeenSet) throw new InvalidUwmfException("Did not set Oldnum on ThingDefinition");
            if (!_actorHasBeenSet) throw new InvalidUwmfException("Did not set Actor on ThingDefinition");
            if (!_anglesHasBeenSet) throw new InvalidUwmfException("Did not set Angles on ThingDefinition");
            if (!_holowallHasBeenSet) throw new InvalidUwmfException("Did not set Holowall on ThingDefinition");
            if (!_pathingHasBeenSet) throw new InvalidUwmfException("Did not set Pathing on ThingDefinition");
            if (!_minskillHasBeenSet) throw new InvalidUwmfException("Did not set Minskill on ThingDefinition");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class TileMappings
    {
        public Dictionary<ushort,AmbushModzone> AmbushModzones { get; } = new Dictionary<ushort,AmbushModzone>();
        public Dictionary<ushort,ChangeTriggerModzone> ChangeTriggerModzones { get; } = new Dictionary<ushort,ChangeTriggerModzone>();
        public Dictionary<ushort,Tile> Tiles { get; } = new Dictionary<ushort,Tile>();
        public Dictionary<ushort,Trigger> Triggers { get; } = new Dictionary<ushort,Trigger>();
        public Dictionary<ushort,Zone> Zones { get; } = new Dictionary<ushort,Zone>();
        public TileMappings() { }
        public TileMappings(
            Dictionary<ushort,AmbushModzone> ambushModzones,
            Dictionary<ushort,ChangeTriggerModzone> changeTriggerModzones,
            Dictionary<ushort,Tile> tiles,
            Dictionary<ushort,Trigger> triggers,
            Dictionary<ushort,Zone> zones)
        {
            AmbushModzones.AddRange(ambushModzones);
            ChangeTriggerModzones.AddRange(changeTriggerModzones);
            Tiles.AddRange(tiles);
            Triggers.AddRange(triggers);
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
        public HashSet<ushort> Elevator { get; } = new HashSet<ushort>();
        public Dictionary<ushort,Trigger> Triggers { get; } = new Dictionary<ushort,Trigger>();
        public List<ThingDefinition> ThingDefinitions { get; } = new List<ThingDefinition>();
        public ThingMappings() { }
        public ThingMappings(
            IEnumerable<ushort> elevator,
            Dictionary<ushort,Trigger> triggers,
            IEnumerable<ThingDefinition> thingDefinitions)
        {
            Elevator.AddRange(elevator);
            Triggers.AddRange(triggers);
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
        public List<string> Ceiling { get; } = new List<string>();
        public List<string> Floor { get; } = new List<string>();
        public FlatMappings() { }
        public FlatMappings(
            IEnumerable<string> ceiling,
            IEnumerable<string> floor)
        {
            Ceiling.AddRange(ceiling);
            Floor.AddRange(floor);
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

}
