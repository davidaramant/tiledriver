// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Gui.Views;
using static System.Windows.Media.Colors;

using UwmfTrigger = Tiledriver.Core.FormatModels.Uwmf.Trigger;

namespace Tiledriver.Gui.ViewModels
{
    public sealed class PushwallVm : MapItemVm
    {
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

        private readonly UwmfTrigger _trigger;
        private readonly Geometry _geometry;
        private readonly SolidColorBrush _fill;
        private readonly SolidColorBrush _stroke;

        public PushwallVm(UwmfTrigger trigger)
        {
            _trigger = trigger;

            _geometry = Geometry.Parse(CirclePath);
            _fill = DarkSlateBlue.ToBrush();
            _stroke = DarkBlue.ToBrush();

            Coordinates = new Point(trigger.X, trigger.Y);
            LayerType = LayerType.Thing;
        }

        public override Path CreatePath(int size)
        {
            Element = new Path
            {
                Height = Height(size),
                Width = Width(size),
                Data = _geometry,
                Fill = _fill,
                Stroke = _stroke,
                StrokeThickness = 2,
                Stretch = Stretch.Uniform,
                RenderTransform = new RotateTransform(0, Width(size) / 2, Height(size) / 2)
            };

            Canvas.SetLeft(Element, Left(size));
            Canvas.SetTop(Element, Top(size));

            return Element;
        }

        public override double Left(double size) => (_trigger.X + 0.5) * size - Width(size) / 2;
        public override double Top(double size) => (_trigger.Y + 0.5) * size - Height(size) / 2;
        public override double Height(double size) => size / 1.6;
        public override double Width(double size) => size / 1.6;

        public override string DetailType => "Push Wall";

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties(null, "Direction", ((Direction)_trigger.Arg3).ToString());
            }
        }

    }
}