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
        private static ThingVm Default => new ThingVm(CIRCLE, Violet, White);
        private static ThingVm PatrolPoint => new ThingVm(ARROW, Black, White, true);
        private static ThingVm Player => new ThingVm(MAN, Black, Yellow, true);
        private static ThingVm EnemyMan(Color fill) => new ThingVm(MAN, fill, White, true);
        private static ThingVm Key(Color fill) => new ThingVm(KEY, fill, fill);
        private static ThingVm PacmanGhost => new ThingVm(PACMAN_GHOST, GhostWhite, LightBlue);
        private static ThingVm Decoration => new ThingVm(CIRCLE, LightGreen, Green);

        public Geometry Geometry { get; set; }
        public SolidColorBrush Fill { get; set; }
        public SolidColorBrush Stroke { get; set; }
        public bool ShouldRotate { get; set; }
        public Thing Thing { get; set; }

        private ThingVm(string path, Color fill, Color stroke, bool shouldRotate = false)
        {
            Geometry = Geometry.Parse(path);
            Fill = fill.ToBrush();
            Stroke = stroke.ToBrush();
            ShouldRotate = shouldRotate;
        }

        public static ThingVm Create(Thing thing, string category)
        {
            var key = ThingVm.ShouldUseCategory.Contains(category) ? category : thing.Type;

            var thingVm = ThingVm.Templates.ContainsKey(key) ? ThingVm.Templates[key] : ThingVm.Default;
            thingVm.Thing = thing;

            return thingVm;
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

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties("Thing", "Type", Thing.Type);
                
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

        private static Dictionary<string, ThingVm> Templates = new Dictionary<string, ThingVm>
        {
            // SPECIAL
            { "$Player1Start", Player },
            { "PatrolPoint", PatrolPoint },
            // GUARDS
            { "DeadGuard", Decoration },
            { "Dog", new ThingVm(DOG, Brown, SaddleBrown) },
            { "Guard", EnemyMan(SaddleBrown) },
            { "Officer", EnemyMan(White) },
            { "WolfensteinSS", EnemyMan(Blue) },
            { "Mutant", EnemyMan(Green) },
            // KEYS
            { "GoldKey", Key(Gold) },
            { "SilverKey", Key(Silver) },
            // THING CATEGORIES
            { "Bosses", EnemyMan(Red) },
            { "Ghosts", PacmanGhost },
            { "Decorations", Decoration },
            { "Treasure", new ThingVm(CROWN, Gold, Gold) },
            { "Health", new ThingVm(CROSS, Blue, White) },
            { "Weapons", new ThingVm(GUN, Gray, LightGray) },
        };
    }
}
