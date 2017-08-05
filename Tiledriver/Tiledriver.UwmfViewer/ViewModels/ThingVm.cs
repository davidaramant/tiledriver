using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.UwmfViewer.Views;
using static System.Windows.Media.Colors;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class ThingVm : MapItemVm
    {
        private readonly Thing thing;
        public bool isPlayer => thing.Type == "$Player1Start";
        private readonly string category;
        private readonly bool shouldRotate;
        public override double Angle() => shouldRotate ? (450 - thing.Angle) % 360 : 0;

        public ThingVm(Thing thing, string category)
        {
            this.thing = thing;
            this.category = category;

            var template = templates.ContainsKey(thing.Type) ? templates[thing.Type] : templates.ContainsKey(category ?? "") ? templates[category] : Default;

            shape = template.Geometry;
            Fill = template.Fill;
            Stroke = template.Stroke;
            shouldRotate = template.ShouldRotate;

            Coordinates = new Point(Math.Floor(thing.X), Math.Floor(thing.Y));
            LayerType = LayerType.Thing;
        }

        public override Path CreatePath(int size)
        {
            var points = Utilities.PointManipulator.CreatePath(shape, Width(64), Height(64), 0, 0, 0);
            var pathMarkup = $"M {String.Join(" ", points)} Z";

            element = new Path()
            {
                Height = Height(size),
                Width = Width(size),
                Data = Geometry.Parse(pathMarkup),
                Fill = Fill.ToBrush(),
                Stroke = Stroke.ToBrush(),
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
        public override double Height(double size) => size / 1.6;
        public override double Width(double size) => size / 1.6;

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

        private static Dictionary<string, ThingVmTemplate> templates = new Dictionary<string, ThingVmTemplate>
        {
            // SPECIAL
            { "$Player1Start", Player() },
            { "PatrolPoint", PatrolPoint() },
            // GUARDS
            { "DeadGuard", Diamond(Brown, SaddleBrown) },
            { "Dog", Dog() },
            { "Guard", EnemyMan(SaddleBrown) },
            { "Officer", EnemyMan(White) },
            { "WolfensteinSS", EnemyMan(Blue) },
            { "Mutant", EnemyMan(Green) },
            // KEYS
            { "GoldKey", Key(Gold) },
            { "SilverKey", Key(Silver) },
            // DECORATIONS
            { "WhitePillar", Diamond(White, LightGray) },
            { "CeilingLight", Diamond(LightYellow, LightGoldenrodYellow) },
            
            // CATEGORIES
            { "Bosses", Boss() },
            { "Ghosts", PacmanGhost() },
            { "Decorations", Diamond(Green, LightGreen) },
            { "Treasure", Treasure() },
            { "Health", Health() },
            { "Weapons", Weapons() },
            { "Ammo", Ammo() },
        };

        private static ThingVmTemplate Default => new ThingVmTemplate(iDIAMOND, Violet, White);
        private static ThingVmTemplate Player() => new ThingVmTemplate(iMAN, Yellow, Yellow, true);
        private static ThingVmTemplate PatrolPoint() => new ThingVmTemplate(iARROW, Black, White, true);
        private static ThingVmTemplate EnemyMan(Color fill) => new ThingVmTemplate(iMAN, fill, White, true);
        private static ThingVmTemplate Boss() => new ThingVmTemplate(iBOSS, Red, White, true);
        private static ThingVmTemplate Key(Color fill) => new ThingVmTemplate(iKEY, fill, fill);
        private static ThingVmTemplate PacmanGhost() => new ThingVmTemplate(iPACMAN_GHOST, GhostWhite, LightBlue);
        private static ThingVmTemplate Diamond(Color fill, Color stroke) => new ThingVmTemplate(iDIAMOND, fill, stroke);
        private static ThingVmTemplate Dog() => new ThingVmTemplate(iDOG, Brown, SaddleBrown, shouldRotate:true);
        private static ThingVmTemplate Treasure() => new ThingVmTemplate(iCROWN, Gold, Gold);
        private static ThingVmTemplate Health() => new ThingVmTemplate(iCROSS, Blue, White);
        private static ThingVmTemplate Weapons() => new ThingVmTemplate(iGUN, Gray, LightGray);
        private static ThingVmTemplate Ammo() => new ThingVmTemplate(iAMMO, Gray, LightGray);

        private class ThingVmTemplate
        {
            public int[] Geometry { get; }
            public Color Fill { get; }
            public Color Stroke { get; }
            public bool ShouldRotate { get; }

            public ThingVmTemplate(int[] geometry, Color fill, Color stroke, bool shouldRotate = false)
            {
                Geometry = geometry;
                Fill = fill;
                Stroke = stroke;
                ShouldRotate = shouldRotate;
            }
        }
    }
}
