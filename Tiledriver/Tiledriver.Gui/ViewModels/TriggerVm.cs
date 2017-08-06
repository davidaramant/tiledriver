// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Gui.Views;
using static System.Windows.Media.Colors;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Gui.ViewModels
{
    public class TriggerVm : MapItemVm
    {
        private readonly Core.FormatModels.Uwmf.Trigger _trigger;
        private readonly string _actionName;
        private readonly Geometry _geometry;
        private readonly SolidColorBrush _fill;
        private readonly SolidColorBrush _stroke;
        private readonly string _argsCategory =  "Args";

        enum Direction
        {
            Right,
            BackRight,
            Back,
            BackLeft,
            Left,
            TowardsLeft,
            Towards,
            TowardsRight,
            East,
            NorthEast,
            North,
            NorthWest,
            West,
            SouthWest,
            South,
            SouthEest,
        }

        enum TeleportNewMapFlag
        {
            KeepDirection = 1,
            KeepPosition
        }

        enum DoorDirection
        {
            EastWest,
            NorthSouth
        }

        public TriggerVm(Core.FormatModels.Uwmf.Trigger trigger)
        {
            this._trigger = trigger;
            this._actionName = trigger.Action;
            var template = SelectTemplate(trigger);

            _geometry = template.Geometry;
            _fill = template.Fill;
            _stroke = template.Stroke;

            Coordinates = new Point(trigger.X, trigger.Y);
            LayerType = LayerType.Trigger;
        }

        public override Path CreatePath(int size)
        {
            Element = new Path()
            {
                Height = Height(size),
                Width = Width(size),
                Data = _geometry,
                Fill = _fill,
                Stroke = _stroke,
                StrokeThickness = 2,
                Stretch = Stretch.Uniform
            };

            Canvas.SetLeft(Element, Left(size));
            Canvas.SetTop(Element, Top(size));

            return Element;
        }

        public override double Left(double size) => (_trigger.X + 0.5) * size - Width(size) / 2;
        public override double Top(double size) => (_trigger.Y + 0.5) * size - Height(size) / 2;
        public override double Height(double size) => size / 1.2;
        public override double Width(double size) => size / 1.2;

        public override string DetailType => "Trigger";


        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties("Info", "Name", _actionName.Replace("_", "__"));
                yield return new DetailProperties("Info", "PlayerUse", _trigger.PlayerUse.ToString());
                yield return new DetailProperties("Info", "MonsterUse", _trigger.MonsterUse.ToString());
                yield return new DetailProperties("Info", "Repeatable", _trigger.Repeatable.ToString());
                yield return new DetailProperties("Info", "PlayerCross", _trigger.PlayerCross.ToString());
                yield return new DetailProperties("Info", "IsSecret", _trigger.Secret.ToString());
                yield return new DetailProperties("Info", "Comment", _trigger.Comment);

                yield return new DetailProperties("Activate", "North", _trigger.ActivateNorth.ToString());
                yield return new DetailProperties("Activate", "South", _trigger.ActivateSouth.ToString());
                yield return new DetailProperties("Activate", "East", _trigger.ActivateEast.ToString());
                yield return new DetailProperties("Activate", "West", _trigger.ActivateWest.ToString());

                if (_actionName == "Door_Open")
                {
                    yield return new DetailProperties(_argsCategory, "Tag", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "Speed", _trigger.Arg1.ToString());
                    yield return new DetailProperties(_argsCategory, "Delay", _trigger.Arg2.ToString());
                    yield return new DetailProperties(_argsCategory, "Lock", ((LockLevel) _trigger.Arg3).ToString());
                    yield return new DetailProperties(_argsCategory, "Type", ((DoorDirection) _trigger.Arg4).ToString());
                }
                else if (_actionName == "Pushwall_Move")
                {
                    yield return new DetailProperties(_argsCategory, "Tag", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "Speed", _trigger.Arg1.ToString());
                    yield return new DetailProperties(_argsCategory, "Direction", ((Direction) _trigger.Arg2).ToString());
                    yield return new DetailProperties(_argsCategory, "Distance", _trigger.Arg3.ToString());
                }
                else if (_actionName == "Teleport_NewMap")
                {
                    yield return new DetailProperties(_argsCategory, "Map", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "Position", _trigger.Arg1.ToString());
                    yield return new DetailProperties(_argsCategory, "Flags", ((TeleportNewMapFlag) _trigger.Arg2).ToString());
                }
                else if (_actionName == "Trigger_Execute")
                {
                    yield return new DetailProperties(_argsCategory, "X", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "Y", _trigger.Arg1.ToString());
                    yield return new DetailProperties(_argsCategory, "Z", _trigger.Arg2.ToString());
                }
                else if (_actionName == "StartConversation")
                {
                    yield return new DetailProperties(_argsCategory, "Tid", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "FaceTalker", _trigger.Arg1.ToString());
                }
                else if (_actionName == "Door_Elevator")
                {
                    yield return new DetailProperties(_argsCategory, "SwitchTag", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "Speed", _trigger.Arg1.ToString());
                    yield return new DetailProperties(_argsCategory, "Delay", _trigger.Arg2.ToString());
                    yield return new DetailProperties(_argsCategory, "Lock", ((LockLevel)_trigger.Arg3).ToString());
                    yield return new DetailProperties(_argsCategory, "Type", ((DoorDirection)_trigger.Arg4).ToString());
                }
                else if (_actionName == "Elevator_SwitchDoor")
                {
                    yield return new DetailProperties(_argsCategory, "ElevTag", _trigger.Arg0.ToString());
                    yield return new DetailProperties(_argsCategory, "DoorTag", _trigger.Arg1.ToString());
                    yield return new DetailProperties(_argsCategory, "CallSpeed", _trigger.Arg2.ToString());
                    yield return new DetailProperties(_argsCategory, "NextTag", _trigger.Arg3.ToString());
                }
            }
        }

        private TriggerVmTemplate SelectTemplate(Core.FormatModels.Uwmf.Trigger trigger)
        {
            var key = trigger.Action;
            if (! _templates.ContainsKey(key)) return Default;
            if (key == "Door_Open" && ((LockLevel)trigger.Arg3 != LockLevel.None))
            {
                key += ((LockLevel)trigger.Arg3);
            }
            return _templates[key];
        }

        private static Dictionary<string, TriggerVmTemplate> _templates = new Dictionary<string, TriggerVmTemplate>
        {
            { "Door_Open", DoorOpen() },
            { "Door_OpenSilver", DoorOpenSilver() },
            { "Door_OpenGold", DoorOpenGold() },
            { "Door_OpenBoth", DoorOpenBoth() },
            { "Door_Open3", DoorOpenBoth() },
            { "Pushwall_Move", PushwallMove() },
            { "Exit_Normal", ExitNormal() },
            { "Exit_Secret", ExitSecret() },
            { "Teleport_NewMap", TeleportNewMap() },
            { "Exit_VictorySpin", ExitVictorySpin() },
            { "Exit_Victory", ExitVictory() },
            { "Trigger_Execute", TriggerExecute() },
            { "StartConversation", StartConversation() },
            { "Door_Elevator", DoorElevator() },
            { "Elevator_SwitchFloor", ElevatorSwitchFloor() }
        };

        private static TriggerVmTemplate Default => new TriggerVmTemplate(SquarePath, Transparent(), White);

        private static TriggerVmTemplate DoorOpen() => new TriggerVmTemplate(SquarePath, Transparent(), Brown);
        private static TriggerVmTemplate DoorOpenSilver() => new TriggerVmTemplate(SquarePath, Transparent(Violet), Brown);
        private static TriggerVmTemplate DoorOpenGold() => new TriggerVmTemplate(SquarePath, Transparent(Gold), Brown);
        private static TriggerVmTemplate DoorOpenBoth() => new TriggerVmTemplate(SquarePath, Transparent(Blue), Brown);
        private static TriggerVmTemplate PushwallMove() => new TriggerVmTemplate(SquarePath, Transparent(), Yellow);
        private static TriggerVmTemplate ExitNormal() => new TriggerVmTemplate(SquarePath, Transparent(), Green);
        private static TriggerVmTemplate ExitSecret() => new TriggerVmTemplate(SquarePath, Transparent(), Blue);
        private static TriggerVmTemplate TeleportNewMap() => new TriggerVmTemplate(SquarePath, Transparent(), DarkGoldenrod);
        private static TriggerVmTemplate ExitVictorySpin() => new TriggerVmTemplate(SquarePath, Transparent(), DarkOrchid);
        private static TriggerVmTemplate ExitVictory() => new TriggerVmTemplate(SquarePath, Transparent(), DarkMagenta);
        private static TriggerVmTemplate TriggerExecute() => new TriggerVmTemplate(SquarePath, Transparent(), DarkOrange);
        private static TriggerVmTemplate StartConversation() => new TriggerVmTemplate(SquarePath, Transparent(), DarkSlateBlue);
        private static TriggerVmTemplate DoorElevator() => new TriggerVmTemplate(SquarePath, Transparent(), DarkSalmon);
        private static TriggerVmTemplate ElevatorSwitchFloor() => new TriggerVmTemplate(SquarePath, Transparent(), DarkMagenta);

        private class TriggerVmTemplate
        {
            public Geometry Geometry { get; }
            public SolidColorBrush Fill { get; }
            public SolidColorBrush Stroke { get; }

            public TriggerVmTemplate(string path, Color fill, Color stroke)
            {
                Geometry = Geometry.Parse(path);
                Fill = fill.ToBrush();
                Stroke = stroke.ToBrush();
            }
        }

        // FIXME There must be a better way to do this.
        private static Color Transparent(Color? nullableColor = null)
        {
            if (nullableColor == null)
            {
                return Color.FromArgb(0, 0, 0, 0);
            }
            Color color = nullableColor ?? Black;
            return Color.FromArgb(127, color.R, color.G, color.B);
        }
    }
}
