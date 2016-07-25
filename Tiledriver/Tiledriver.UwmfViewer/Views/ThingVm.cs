using System;
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

        private ThingVm(Thing thing, string category, string path, Color fill, Color stroke, bool shouldRotate = false)
        {
            this.thing = thing;
            this.category = category;
            geometry = Geometry.Parse(path);
            this.fill = fill.ToBrush();
            this.stroke = stroke.ToBrush();
            this.shouldRotate = shouldRotate;
        }

        public static ThingVm Create(Thing thing, string category)
        {
            var key = shouldUseCategory.Contains(category) ? category : thing.Type;

            if (templates.ContainsKey(key))
            {
                var thingVm = templates[key](thing, category);
                return thingVm;
            }
            else
            {
                var thingVm = Default(thing, category);
                return thingVm;
            }
        }

        public override UIElement ToUIElement(int size)
        {
            var element = new Path()
            {
                Height = size / 2,
                Width = size / 2,
                Data = geometry,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = 2,
                Stretch = Stretch.Uniform
            };

            if (shouldRotate)
            {
                element.RenderTransform = new RotateTransform((450 - thing.Angle) % 360, element.Width / 2, element.Height / 2);
            }

            Canvas.SetLeft(element, thing.X * size - element.Width / 2);
            Canvas.SetTop(element, thing.Y * size - element.Height / 2);

            return element;
        }

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

        private static Dictionary<string, Func<Thing,string,ThingVm>> templates = new Dictionary<string, Func<Thing,string,ThingVm>>
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

        private static Func<Thing,string,ThingVm> Default = (t,c) => new ThingVm(t, c, CIRCLE, Violet, White);
        private static Func<Thing,string,ThingVm> PatrolPoint() => (t,c) => new ThingVm(t, c, ARROW, Black, White, true);
        private static Func<Thing,string,ThingVm> Player() => (t,c) => new ThingVm(t, c, MAN, Black, Yellow, true);
        private static Func<Thing,string,ThingVm> EnemyMan(Color fill) => (t,c) => new ThingVm(t, c, MAN, fill, White, true);
        private static Func<Thing,string,ThingVm> Key(Color fill) => (t,c) => new ThingVm(t, c, KEY, fill, fill);
        private static Func<Thing,string,ThingVm> PacmanGhost() => (t,c) => new ThingVm(t, c, PACMAN_GHOST, GhostWhite, LightBlue);
        private static Func<Thing,string,ThingVm> Decoration() => (t,c) => new ThingVm(t, c, CIRCLE, LightGreen, Green);
        private static Func<Thing,string,ThingVm> Dog() => (t,c) => new ThingVm(t, c, DOG, Brown, SaddleBrown);
        private static Func<Thing,string,ThingVm> Treasure() => (t,c) => new ThingVm(t, c, CROWN, Gold, Gold);
        private static Func<Thing,string,ThingVm> Health() => (t,c) => new ThingVm(t, c, CROSS, Blue, White);
        private static Func<Thing,string,ThingVm> Weapons() => (t,c) => new ThingVm(t, c, GUN, Gray, LightGray);
        private static Func<Thing, string, ThingVm> Ammo() => (t,c) => new ThingVm(t, c, AMMO, Gray, LightGray);
    }
}
