// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.Extensions;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class Elevator : IThingMapping
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
            }
        }
        public Elevator() { }
        public Elevator(
            ushort oldNum)
        {
            OldNum = oldNum;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on Elevator");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ThingTemplate : IThingMapping
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
            }
        }
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
        public ThingTemplate() { }
        public ThingTemplate(
            ushort oldNum,
            string type,
            int angles,
            bool holowall,
            bool pathing,
            bool ambush,
            int minskill)
        {
            OldNum = oldNum;
            Type = type;
            Angles = angles;
            Holowall = holowall;
            Pathing = pathing;
            Ambush = ambush;
            Minskill = minskill;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on ThingTemplate");
            if (!_typeHasBeenSet) throw new InvalidUwmfException("Did not set Type on ThingTemplate");
            if (!_anglesHasBeenSet) throw new InvalidUwmfException("Did not set Angles on ThingTemplate");
            if (!_holowallHasBeenSet) throw new InvalidUwmfException("Did not set Holowall on ThingTemplate");
            if (!_pathingHasBeenSet) throw new InvalidUwmfException("Did not set Pathing on ThingTemplate");
            if (!_ambushHasBeenSet) throw new InvalidUwmfException("Did not set Ambush on ThingTemplate");
            if (!_minskillHasBeenSet) throw new InvalidUwmfException("Did not set Minskill on ThingTemplate");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class TriggerTemplate : IThingMapping
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
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
        public TriggerTemplate() { }
        public TriggerTemplate(
            ushort oldNum,
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
            OldNum = oldNum;
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
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on TriggerTemplate");
            if (!_actionHasBeenSet) throw new InvalidUwmfException("Did not set Action on TriggerTemplate");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class AmbushModzone
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
            }
        }
        public bool Fillzone { get; set; } = false;
        public AmbushModzone() { }
        public AmbushModzone(
            ushort oldNum,
            bool fillzone = false)
        {
            OldNum = oldNum;
            Fillzone = fillzone;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on AmbushModzone");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ChangeTriggerModzone
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
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
        private bool _triggerTemplateHasBeenSet = false;
        private TriggerTemplate _triggerTemplate;
        public TriggerTemplate TriggerTemplate
        {
            get { return _triggerTemplate; }
            set
            {
                _triggerTemplateHasBeenSet = true;
                _triggerTemplate = value;
            }
        }
        public bool Fillzone { get; set; } = false;
        public ChangeTriggerModzone() { }
        public ChangeTriggerModzone(
            ushort oldNum,
            string action,
            TriggerTemplate triggerTemplate,
            bool fillzone = false)
        {
            OldNum = oldNum;
            Action = action;
            TriggerTemplate = triggerTemplate;
            Fillzone = fillzone;
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on ChangeTriggerModzone");
            if (!_actionHasBeenSet) throw new InvalidUwmfException("Did not set Action on ChangeTriggerModzone");
            if (!_triggerTemplateHasBeenSet) throw new InvalidUwmfException("Did not set TriggerTemplate on ChangeTriggerModzone");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class TileTemplate
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
            }
        }
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
        public TileTemplate() { }
        public TileTemplate(
            ushort oldNum,
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
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            OldNum = oldNum;
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
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on TileTemplate");
            if (!_textureEastHasBeenSet) throw new InvalidUwmfException("Did not set TextureEast on TileTemplate");
            if (!_textureNorthHasBeenSet) throw new InvalidUwmfException("Did not set TextureNorth on TileTemplate");
            if (!_textureWestHasBeenSet) throw new InvalidUwmfException("Did not set TextureWest on TileTemplate");
            if (!_textureSouthHasBeenSet) throw new InvalidUwmfException("Did not set TextureSouth on TileTemplate");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class ZoneTemplate
    {
        private bool _oldNumHasBeenSet = false;
        private ushort _oldNum;
        public ushort OldNum
        {
            get { return _oldNum; }
            set
            {
                _oldNumHasBeenSet = true;
                _oldNum = value;
            }
        }
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();
        public ZoneTemplate() { }
        public ZoneTemplate(
            ushort oldNum,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            OldNum = oldNum;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }
        public void CheckSemanticValidity()
        {
            if (!_oldNumHasBeenSet) throw new InvalidUwmfException("Did not set OldNum on ZoneTemplate");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();
    }

    public sealed partial class TileMappings
    {
        public List<AmbushModzone> AmbushModzones { get; } = new List<AmbushModzone>();
        public List<ChangeTriggerModzone> ChangeTriggerModzones { get; } = new List<ChangeTriggerModzone>();
        public List<TileTemplate> TileTemplates { get; } = new List<TileTemplate>();
        public List<TriggerTemplate> TriggerTemplates { get; } = new List<TriggerTemplate>();
        public List<ZoneTemplate> ZoneTemplates { get; } = new List<ZoneTemplate>();
        public TileMappings() { }
        public TileMappings(
            IEnumerable<AmbushModzone> ambushModzones,
            IEnumerable<ChangeTriggerModzone> changeTriggerModzones,
            IEnumerable<TileTemplate> tileTemplates,
            IEnumerable<TriggerTemplate> triggerTemplates,
            IEnumerable<ZoneTemplate> zoneTemplates)
        {
            AmbushModzones.AddRange(ambushModzones);
            ChangeTriggerModzones.AddRange(changeTriggerModzones);
            TileTemplates.AddRange(tileTemplates);
            TriggerTemplates.AddRange(triggerTemplates);
            ZoneTemplates.AddRange(zoneTemplates);
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
