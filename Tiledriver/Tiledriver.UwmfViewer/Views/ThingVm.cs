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

        public Geometry Geometry { get; set; }
        public SolidColorBrush Fill { get; set; }
        public SolidColorBrush Stroke { get; set; }
        public bool ShouldRotate { get; set; }
        public Thing Thing { get; set; }
        public string Category { get; set; }

        private ThingVm(Thing thing, string category, string path, Color fill, Color stroke, bool shouldRotate = false)
        {
            Geometry = Geometry.Parse(path);
            Fill = fill.ToBrush();
            Stroke = stroke.ToBrush();
            ShouldRotate = shouldRotate;
            Thing = thing;
            Category = category;
        }

        public static ThingVm Create(Thing thing, string category)
        {
            var key = ShouldUseCategory.Contains(category) ? category : thing.Type;

            if (Templates.ContainsKey(key))
            {
                var thingVm = Templates[key](thing, category);
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
                Data = Geometry,
                Fill = Fill,
                Stroke = Stroke,
                StrokeThickness = 2,
                Stretch = Stretch.Uniform
            };

            if (ShouldRotate)
            {
                element.RenderTransform = new RotateTransform((450 - Thing.Angle) % 360, element.Width / 2, element.Height / 2);
            }

            Canvas.SetLeft(element, Thing.X * size - element.Width / 2);
            Canvas.SetTop(element, Thing.Y * size - element.Height / 2);

            return element;
        }

        public override string DetailType => Thing?.Type ?? "Thing";

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties(null, "Category", Category);

                yield return new DetailProperties("Position", "X", Thing.X.ToString());
                yield return new DetailProperties("Position", "Y", Thing.Y.ToString());
                yield return new DetailProperties("Position", "Angle", Thing.Angle.ToString());

                yield return new DetailProperties("Skill Level", "Level 1", Thing.Skill1 ? "Yes" : "No");
                yield return new DetailProperties("Skill Level", "Level 2", Thing.Skill2 ? "Yes" : "No");
                yield return new DetailProperties("Skill Level", "Level 3", Thing.Skill3 ? "Yes" : "No");
                yield return new DetailProperties("Skill Level", "Level 4", Thing.Skill4 ? "Yes" : "No");

                yield return new DetailProperties("Special", "Ambush", Thing.Ambush ? "Yes" : "No");
                yield return new DetailProperties("Special", "Patrol", Thing.Patrol ? "Yes" : "No");
            }
        }

        public static List<string> ShouldUseCategory = new List<string>
        {
            "Bosses",
            "Decorations",
            "Ammo",
            "Treasure",
            "Ghosts",
            "Health",
            "Weapons",
        };

        private static Dictionary<string, Func<Thing,string,ThingVm>> Templates = new Dictionary<string, Func<Thing,string,ThingVm>>
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
