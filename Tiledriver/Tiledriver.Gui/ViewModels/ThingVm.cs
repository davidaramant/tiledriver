// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;
using Tiledriver.Gui.Views;
using static System.Windows.Media.Colors;

namespace Tiledriver.Gui.ViewModels
{
    public class ThingVm : MapItemVm
    {
        private readonly Thing _thing;
        private readonly string _category;
        private readonly Geometry _geometry;
        private readonly SolidColorBrush _fill;
        private readonly SolidColorBrush _stroke;
        private readonly bool _shouldRotate;

        public ThingVm(Thing thing, string category)
        {
            this._thing = thing;
            this._category = category;

            var template = _templates.ContainsKey(thing.Type) ? _templates[thing.Type] : _templates.ContainsKey(category ?? "") ? _templates[category] : Default;

            _geometry = template.Geometry;
            _fill = template.Fill;
            _stroke = template.Stroke;
            _shouldRotate = template.ShouldRotate;

            Coordinates = new Point(Math.Floor(thing.X), Math.Floor(thing.Y));
            LayerType = LayerType.Thing;
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

        public override IEnumerable<DetailProperties> Details
        {
            get
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
        }

        private static Dictionary<string, ThingVmTemplate> _templates = new Dictionary<string, ThingVmTemplate>
        {
            // SPECIAL
            { "$Player1Start", Player() },
            { "PatrolPoint", PatrolPoint() },
            { Actor.Blinky.ClassName, PacmanGhost(Red) },
            { Actor.Pinky.ClassName, PacmanGhost(Color.FromRgb(255,184,255)) },
            { Actor.Inky.ClassName, PacmanGhost(Cyan) },
            { Actor.Clyde.ClassName, PacmanGhost(Color.FromRgb(255,184,81)) },
            // GUARDS
            { "DeadGuard", Circle(Brown, SaddleBrown) },
            { "Dog", Dog() },
            { "Guard", EnemyMan(SaddleBrown) },
            { "Officer", EnemyMan(White) },
            { "WolfensteinSS", EnemyMan(Blue) },
            { "Mutant", EnemyMan(Green) },
            // KEYS
            { "GoldKey", Key(Gold) },
            { "SilverKey", Key(Silver) },
            // DECORATIONS
            { "WhitePillar", Circle(White, LightGray) },
            { "CeilingLight", Circle(DarkOrange, DarkGoldenrod) },
            
            // CATEGORIES
            { "Bosses", Boss() },
            { "Decorations", Circle(DarkGreen, Green) },
            { "Treasure", Treasure() },
            { "Health", Health() },
            { "Weapons", Weapons() },
            { "Ammo", Ammo() },
        };

        private static ThingVmTemplate Default => new ThingVmTemplate(CirclePath, Violet, White);

        private static ThingVmTemplate Player() => new ThingVmTemplate(ManPath, Fuchsia, DeepPink, true);
        private static ThingVmTemplate PatrolPoint() => new ThingVmTemplate(ArrowPath, Black, LightGray, true);
        private static ThingVmTemplate EnemyMan(Color fill) => new ThingVmTemplate(ManPath, fill, Red, true);
        private static ThingVmTemplate Boss() => new ThingVmTemplate(BossPath, Fuchsia, Fuchsia, true);
        private static ThingVmTemplate Key(Color fill) => new ThingVmTemplate(KeyPath, fill, fill);
        private static ThingVmTemplate PacmanGhost(Color color) => new ThingVmTemplate(PacmanGhostPath, color, GhostWhite);
        private static ThingVmTemplate Circle(Color fill, Color stroke) => new ThingVmTemplate(CirclePath, fill, stroke);
        private static ThingVmTemplate Dog() => new ThingVmTemplate(DogPath, Brown, SaddleBrown, shouldRotate: true);
        private static ThingVmTemplate Treasure() => new ThingVmTemplate(CrownPath, Gold, DarkGoldenrod);
        private static ThingVmTemplate Health() => new ThingVmTemplate(CrossPath, Blue, White);
        private static ThingVmTemplate Weapons() => new ThingVmTemplate(GunPath, Gray, DarkGray);
        private static ThingVmTemplate Ammo() => new ThingVmTemplate(AmmoPath, Gray, DarkGray);

        private class ThingVmTemplate
        {
            public Geometry Geometry { get; }
            public SolidColorBrush Fill { get; }
            public SolidColorBrush Stroke { get; }
            public bool ShouldRotate { get; }

            public ThingVmTemplate(string path, Color fill, Color stroke, bool shouldRotate = false)
            {
                Geometry = Geometry.Parse(path);
                Fill = fill.ToBrush();
                Stroke = stroke.ToBrush();
                ShouldRotate = shouldRotate;
            }
        }
    }
}
