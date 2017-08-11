// Copyright (c) 2017, David Aramant and Luke Gilbert
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Gui.Views;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Gui.ViewModels
{
    public sealed class TriggerVm : MapItemVm
    {
        private readonly Core.FormatModels.Uwmf.Trigger _trigger;
        private readonly string _actionName;
        private readonly Geometry _geometry;
        private readonly SolidColorBrush _fill;
        private readonly SolidColorBrush _stroke;
        private readonly string _argsCategory = "Args";

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
        public override double Height(double size) => size / 3.0;
        public override double Width(double size) => size / 3.0;

        public override string DetailType => "Trigger";


        public override IEnumerable<DetailProperties> GetDetails()
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
                yield return new DetailProperties(_argsCategory, "Lock", ((LockLevel)_trigger.Arg3).ToString());
                yield return new DetailProperties(_argsCategory, "Type", ((DoorDirection)_trigger.Arg4).ToString());
            }
            else if (_actionName == "Pushwall_Move")
            {
                yield return new DetailProperties(_argsCategory, "Tag", _trigger.Arg0.ToString());
                yield return new DetailProperties(_argsCategory, "Speed", _trigger.Arg1.ToString());
                yield return new DetailProperties(_argsCategory, "Direction", ((Direction)_trigger.Arg2).ToString());
                yield return new DetailProperties(_argsCategory, "Distance", _trigger.Arg3.ToString());
            }
            else if (_actionName == "Teleport_NewMap")
            {
                yield return new DetailProperties(_argsCategory, "Map", _trigger.Arg0.ToString());
                yield return new DetailProperties(_argsCategory, "Position", _trigger.Arg1.ToString());
                yield return new DetailProperties(_argsCategory, "Flags", ((TeleportNewMapFlag)_trigger.Arg2).ToString());
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

        private Template DecoratedDoor(Core.FormatModels.Uwmf.Trigger trigger)
        {
            var lockLevel = (LockLevel)trigger.Arg3;
            if (lockLevel == LockLevel.Gold) return new Template(GeometryCache.Circle, Colors.Gold, Colors.Black);
            if (lockLevel == LockLevel.Silver) return new Template(GeometryCache.Circle, Colors.Silver, Colors.Black);
            if (lockLevel == LockLevel.Both) return new Template(GeometryCache.Circle, Colors.Blue, Colors.Black);
            return Default;
        }

        private Template SelectTemplate(Core.FormatModels.Uwmf.Trigger trigger)
        {
            var key = trigger.Action;
            var requiresKey = ((LockLevel)trigger.Arg3 != LockLevel.None);
            if ((key == "Door_Open") && requiresKey)
            {
                return DecoratedDoor(trigger);
            }
            return Templates.ContainsKey(key) ? Templates[key] : Default;
        }

        private static readonly Dictionary<string, Template> Templates = new Dictionary<string, Template>
        {
            { "Door_Open", DoorOpen() },
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

        private static Template Default => new Template(GeometryCache.Square, Colors.Transparent, Colors.White);

        private static Template DoorOpen() => new Template(GeometryCache.NoPath, Colors.Transparent, Colors.Brown);
        private static Template PushwallMove() => new Template(GeometryCache.Square, Colors.Transparent, Colors.Yellow);
        private static Template ExitNormal() => new Template(GeometryCache.Square, Colors.Transparent, Colors.Green);
        private static Template ExitSecret() => new Template(GeometryCache.Square, Colors.Transparent, Colors.Blue);
        private static Template TeleportNewMap() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkGoldenrod);
        private static Template ExitVictorySpin() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkOrchid);
        private static Template ExitVictory() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkMagenta);
        private static Template TriggerExecute() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkOrange);
        private static Template StartConversation() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkSlateBlue);
        private static Template DoorElevator() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkSalmon);
        private static Template ElevatorSwitchFloor() => new Template(GeometryCache.Square, Colors.Transparent, Colors.DarkMagenta);

        private sealed class Template
        {
            public Geometry Geometry { get; }
            public SolidColorBrush Fill { get; }
            public SolidColorBrush Stroke { get; }

            public Template(Geometry geometry, Color fill, Color stroke)
            {
                Geometry = geometry;
                Fill = fill.ToBrush();
                Stroke = stroke.ToBrush();
            }
        }
    }
}
