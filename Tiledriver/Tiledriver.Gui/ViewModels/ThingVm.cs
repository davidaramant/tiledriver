// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;
using Tiledriver.Gui.Views;

namespace Tiledriver.Gui.ViewModels
{
    public sealed class ThingVm : MapItemVm
    {
        private readonly Thing _thing;
        private readonly string _category;
        private readonly Geometry _geometry;
        private readonly SolidColorBrush _fill;
        private readonly SolidColorBrush _stroke;
        private readonly bool _shouldRotate;

        public ThingVm(Thing thing, string category)
        {
            _thing = thing;
            _category = category;

            var template =
                ThingTemplates.
                Lookup(thing.Type).
                Or(() => CategoryTemplates.Lookup(category ?? string.Empty)).
                OrElse(Unknown);

            _geometry = template.Geometry;
            _fill = template.Fill;
            _stroke = template.Stroke;
            _shouldRotate = template.ShouldRotate;

            Coordinates = new Point(Math.Floor(thing.X), Math.Floor(thing.Y));
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
                StrokeThickness = 1,
                Stretch = Stretch.Uniform
            };

            if (_shouldRotate)
            {
                Element.RenderTransform = new RotateTransform((450 - _thing.Angle) % 360, Width(size) / 2, Height(size) / 2);
            }

            Canvas.SetLeft(Element, Left(size));
            Canvas.SetTop(Element, Top(size));

            return Element;
        }

        public override double Left(double size) => _thing.X * size - Width(size) / 2;
        public override double Top(double size) => _thing.Y * size - Height(size) / 2;
        public override double Height(double size) => size / 1.6;
        public override double Width(double size) => size / 1.6;

        public override string DetailType => _thing?.Type ?? "Thing";

        public override IEnumerable<DetailProperties> GetDetails()
        {
            yield return new DetailProperties(null, "Category", _category);

            yield return new DetailProperties("Position", "Angle", (_thing.Angle % 360).ToString());

            yield return new DetailProperties("Skill Level", "Level 1", _thing.Skill1 ? "Yes" : "No");
            yield return new DetailProperties("Skill Level", "Level 2", _thing.Skill2 ? "Yes" : "No");
            yield return new DetailProperties("Skill Level", "Level 3", _thing.Skill3 ? "Yes" : "No");
            yield return new DetailProperties("Skill Level", "Level 4", _thing.Skill4 ? "Yes" : "No");

            yield return new DetailProperties("Special", "Ambush", _thing.Ambush ? "Yes" : "No");
            yield return new DetailProperties("Special", "Patrol", _thing.Patrol ? "Yes" : "No");
        }

        private static readonly Dictionary<string, Template> ThingTemplates = new Dictionary<string, Template>
        {
            // SPECIAL
            { Actor.Player1Start.ClassName, Player() },
            { Actor.PatrolPoint.ClassName, PatrolPoint() },
            // Ghosts
            { Actor.Blinky.ClassName, PacmanGhost(Colors.Red) },
            { Actor.Pinky.ClassName, PacmanGhost(Color.FromRgb(255,184,255)) },
            { Actor.Inky.ClassName, PacmanGhost(Colors.Cyan) },
            { Actor.Clyde.ClassName, PacmanGhost(Color.FromRgb(255,184,81)) },
            
            // KEYS
            { Actor.GoldKey.ClassName, Key(Colors.Gold) },
            { Actor.SilverKey.ClassName, Key(Colors.Silver) },            
        };

        private static readonly Dictionary<string, Template> CategoryTemplates = new Dictionary<string, Template>
        {
            { CategoryType.Enemy, Enemy() },
            { CategoryType.Bosses, Boss() },
            { CategoryType.Decorations, Circle(Colors.DarkGreen, Colors.Green) },
            { CategoryType.Treasure, Treasure() },
            { CategoryType.Health, Health() },
            { CategoryType.Weapons, Weapons() },
            { CategoryType.Ammo, Ammo() },
        };

        private static Template Unknown => new Template(GeometryCache.Diamond, Colors.DarkRed, Colors.Orange);

        private static Template Player() => new Template(GeometryCache.Man, Colors.Fuchsia, Colors.DeepPink, true);
        private static Template PatrolPoint() => new Template(GeometryCache.Arrow, Colors.Black, Colors.LightGray, true);
        private static Template Enemy() => new Template(GeometryCache.JaggedArrow, Colors.Red, Colors.White, true);

        private static Template EnemyMan(Color fill) => new Template(GeometryCache.Man, fill, Colors.Red, true);
        private static Template Boss() => new Template(GeometryCache.Boss, Colors.Fuchsia, Colors.Fuchsia, true);
        private static Template Key(Color fill) => new Template(GeometryCache.Key, fill, fill);
        private static Template PacmanGhost(Color color) => new Template(GeometryCache.Ghost, color, Colors.GhostWhite);
        private static Template Circle(Color fill, Color stroke) => new Template(GeometryCache.Circle, fill, stroke);
        private static Template Dog() => new Template(GeometryCache.JaggedArrow, Colors.Brown, Colors.SaddleBrown, shouldRotate: true);
        private static Template Treasure() => new Template(GeometryCache.Crown, Colors.Gold, Colors.DarkGoldenrod);
        private static Template Health() => new Template(GeometryCache.Cross, Colors.Blue, Colors.White);
        private static Template Weapons() => new Template(GeometryCache.Gun, Colors.Gray, Colors.DarkGray);
        private static Template Ammo() => new Template(GeometryCache.Ammo, Colors.Gray, Colors.DarkGray);

        private sealed class Template
        {
            public Geometry Geometry { get; }
            public SolidColorBrush Fill { get; }
            public SolidColorBrush Stroke { get; }
            public bool ShouldRotate { get; }

            public Template(Geometry geometry, Color fill, Color stroke, bool shouldRotate = false)
            {
                Geometry = geometry;
                Fill = fill.ToBrush();
                Stroke = stroke.ToBrush();
                ShouldRotate = shouldRotate;
            }
        }
    }
}
