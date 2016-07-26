﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using static System.Windows.Media.Colors;

namespace Tiledriver.UwmfViewer.Views
{
    public class ThingVm : MapItem
    {
        private readonly Thing thing;
        private readonly string category;
        private readonly Geometry geometry;
        private readonly SolidColorBrush fill;
        private readonly SolidColorBrush stroke;
        private readonly bool shouldRotate;

        public ThingVm(Thing thing, string category)
        {
            this.thing = thing;
            this.category = category;

            var key = shouldUseCategory.Contains(category) ? category : thing.Type;
            var template = templates.ContainsKey(key) ? templates[key] : Default;

            geometry = template.Geometry;
            fill = template.Fill;
            stroke = template.Stroke;
            shouldRotate = template.ShouldRotate;
        }

        public override Path CreatePath(int size)
        {
            element = new Path()
            {
                Height = Height(size),
                Width = Width(size),
                Data = geometry,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = 2,
                Stretch = Stretch.Uniform
            };

            if (shouldRotate)
            {
                element.RenderTransform = new RotateTransform((450 - thing.Angle) % 360, Width(size) / 2, Height(size) / 2);
            }

            Canvas.SetLeft(element, Left(size));
            Canvas.SetTop(element, Top(size));

            return element;
        }

        public override double Left(double size) => thing.X * size - Width(size) / 2;
        public override double Top(double size) => thing.Y * size - Height(size) / 2;
        public override double Height(double size) => size / 2;
        public override double Width(double size) => size / 2;

        public override string DetailType => thing?.Type ?? "Thing";

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties(null, "Category", category);

                yield return new DetailProperties("Position", "X", thing.X.ToString());
                yield return new DetailProperties("Position", "Y", thing.Y.ToString());
                yield return new DetailProperties("Position", "Angle", thing.Angle.ToString());

                yield return new DetailProperties("Skill Level", "Level 1", thing.Skill1 ? "Yes" : "No");
                yield return new DetailProperties("Skill Level", "Level 2", thing.Skill2 ? "Yes" : "No");
                yield return new DetailProperties("Skill Level", "Level 3", thing.Skill3 ? "Yes" : "No");
                yield return new DetailProperties("Skill Level", "Level 4", thing.Skill4 ? "Yes" : "No");

                yield return new DetailProperties("Special", "Ambush", thing.Ambush ? "Yes" : "No");
                yield return new DetailProperties("Special", "Patrol", thing.Patrol ? "Yes" : "No");
            }
        }

        private static List<string> shouldUseCategory = new List<string>
        {
            "Bosses",
            "Decorations",
            "Ammo",
            "Treasure",
            "Ghosts",
            "Health",
            "Weapons",
        };

        private static Dictionary<string, ThingVmTemplate> templates = new Dictionary<string, ThingVmTemplate>
        {
            // SPECIAL
            { "$Player1Start", Player() },
            { "PatrolPoint", PatrolPoint() },
            // GUARDS
            { "DeadGuard", Decoration() },
            { "Dog", Dog() },
            { "Guard", EnemyMan(SaddleBrown) },
            { "Officer", EnemyMan(White) },
            { "WolfensteinSS", EnemyMan(Blue) },
            { "Mutant", EnemyMan(Green) },
            // KEYS
            { "GoldKey", Key(Gold) },
            { "SilverKey", Key(Silver) },
            // THING CATEGORIES
            { "Bosses", EnemyMan(Red) },
            { "Ghosts", PacmanGhost() },
            { "Decorations", Decoration() },
            { "Treasure", Treasure() },
            { "Health", Health() },
            { "Weapons", Weapons() },
            { "Ammo", Ammo() },
        };

        private static ThingVmTemplate Default => new ThingVmTemplate(CIRCLE, Violet, White);

        private static ThingVmTemplate Player() => new ThingVmTemplate(MAN, Black, Yellow, true);
        private static ThingVmTemplate PatrolPoint() => new ThingVmTemplate(ARROW, Black, White, true);
        private static ThingVmTemplate EnemyMan(Color fill) => new ThingVmTemplate(MAN, fill, White, true);
        private static ThingVmTemplate Key(Color fill) => new ThingVmTemplate(KEY, fill, fill);
        private static ThingVmTemplate PacmanGhost() => new ThingVmTemplate(PACMAN_GHOST, GhostWhite, LightBlue);
        private static ThingVmTemplate Decoration() => new ThingVmTemplate(CIRCLE, LightGreen, Green);
        private static ThingVmTemplate Dog() => new ThingVmTemplate(DOG, Brown, SaddleBrown);
        private static ThingVmTemplate Treasure() => new ThingVmTemplate(CROWN, Gold, Gold);
        private static ThingVmTemplate Health() => new ThingVmTemplate(CROSS, Blue, White);
        private static ThingVmTemplate Weapons() => new ThingVmTemplate(GUN, Gray, LightGray);
        private static ThingVmTemplate Ammo() => new ThingVmTemplate(AMMO, Gray, LightGray);

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
