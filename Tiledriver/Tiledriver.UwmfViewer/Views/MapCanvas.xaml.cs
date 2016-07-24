using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Wolf3D;
using static System.Console;
using static System.Windows.Media.Colors;

namespace Tiledriver.UwmfViewer.Views
{
    /// <summary>
    /// Interaction logic for MapCanvas.xaml
    /// </summary>
    public partial class MapCanvas : UserControl
    {
        private int squareSize = 32;
        private SquareFactory squareFactory;
        private IEnumerable<Actor> actors = Actor.GetAll().Where(a => a.Wolf3D);

        public MapCanvas()
        {
            InitializeComponent();
        }

        public void Update(Map map)
        {
            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            squareFactory = new SquareFactory(map, squareSize);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Add(squareFactory.ForCoordinates(x, y).ToUIElements());
                }
            }

            map.Things.Select(t => t.Type).Distinct().ToList().ForEach(WriteLine);

            map.Things.ForEach(t =>
            {
                var element = Thing(t.Type, t.Angle);
                Canvas.SetLeft(element, t.X * squareSize - element.Width / 2);
                Canvas.SetTop(element, t.Y * squareSize - element.Height / 2);
                Add(element);
            });
        }

        private Path Thing(string type, int angle)
        {
            var thingVm = GetThingPropertiesFor(type);

            var element = new Path()
            {
                Height = squareSize / 2,
                Width = squareSize / 2,
                Data = thingVm.Geometry,
                Fill = thingVm.Fill,
                Stroke = thingVm.Stroke,
                StrokeThickness = 2,
                Stretch = Stretch.Uniform
            };

            if (thingVm.ShouldRotate)
            {
                element.RenderTransform = new RotateTransform(angle, element.Width / 2, element.Height / 2);
            }

            return element;
        }


        public void Add(UIElement element)
        {
            FullArea.Children.Add(element);
        }

        public void Add(List<UIElement> elements)
        {
            elements.ForEach(Add);
        }

        public List<string> shouldUseCategory = new List<string>
        {
            "Bosses",
            "Decorations",
            "Ammo",
            "Treasure",
            "Ghosts",
            "Health",
            "Weapons",
        };

        public Dictionary<string, ThingVm> thingVms = new Dictionary<string, ThingVm>
        {
            // SPECIAL
            { "$Player1Start", ThingVm.Player },
            { "PatrolPoint", ThingVm.PatrolPoint },
            // GUARDS
            { "Guard", ThingVm.EnemyMan(SaddleBrown) },
            { "Officer", ThingVm.EnemyMan(White) },
            { "Dog", new ThingVm(ThingVm.DOG, Brown, SaddleBrown) }, //TODO
            { "WolfensteinSS", ThingVm.EnemyMan(Blue) },
            { "Mutant", ThingVm.EnemyMan(Green) },
            { "DeadGuard", new ThingVm(ThingVm.CIRCLE, LightGreen, Green) },
            // KEYS
            { "GoldKey", ThingVm.Key(Gold) },
            { "SilverKey", ThingVm.Key(Silver) },

            { "Bosses", ThingVm.EnemyMan(Red) },
            { "Ghosts", ThingVm.PacmanGhost },
            { "Decorations", new ThingVm(ThingVm.CIRCLE, LightGreen, Green) },
            { "Treasure", new ThingVm(ThingVm.CROWN, Gold, Gold) },
            { "Health", new ThingVm(ThingVm.CROSS, Red, White) },
            { "Weapons", new ThingVm(ThingVm.GUN, Gray, LightGray) },
        };

        public ThingVm GetThingPropertiesFor(string type)
        {
            var category = actors.SingleOrDefault(a => a.ClassName == type)?.Category;
            var key = shouldUseCategory.Contains(category) ? category : type;

            return thingVms.ContainsKey(key) ? thingVms[key] : ThingVm.Default;
        }
    }

    public class ThingVm
    {
        public const string TRIANGLE = "M 16 0 L 28 32 L 4 32 Z";
        public const string DIAMOND = "M 1 0 L 2 2 L 1 4 L 0 2 Z";
        public const string CIRCLE = "M0,0 A50,50 0 0 0 0,100 A50,50 0 0 0 0,0";
        public const string CROSS = "M 16 0 L 28 32 L 4 32 Z"; //TODO
        public const string GUN = "M 16 0 L 28 32 L 4 32 Z"; //TODO
        public const string DOG = "M 16 0 L 28 32 L 4 32 Z"; //TODO
        public const string CROWN = "M 1 9 L 0 0 L 4 4 L 6 0 L 8 4 L 12 0 L 11 9 Z";
        public const string MAN = "M 8 0 L 14 6 L 16 9 L 13 10 L 16 16 L 0 16 L 3 10 L 0 9 L 2 6 Z";
        public const string ARROW = "M 8 0 L 16 8 L 11 8 L 11 16 L 5 16 L 5 8 L 0 8 Z";
        public const string PACMAN_GHOST = "M 8 0 L 12 2 L 14 6 L 16 16 L 13 16 L 11 12 L 9 16 L 7 16 L 5 12 L 3 16 L 0 16 L 2 6 L 4 2 Z";
        public const string KEY = "M 0 12 L 0 7 L 9 7 L 12 4 L 16 8 L 12 12 L 9 9 L 2 9 L 2 12 Z";

        public Geometry Geometry { get; set; }
        public SolidColorBrush Fill { get; set; }
        public SolidColorBrush Stroke { get; set; }
        public bool ShouldRotate { get; set; }

        public static ThingVm Default => new ThingVm(CIRCLE, Violet, White);
        public static ThingVm PatrolPoint => new ThingVm(ARROW, Black, White, true);
        public static ThingVm Player => new ThingVm(MAN, Black, Yellow, true);
        public static ThingVm EnemyMan(Color fill) => new ThingVm(MAN, fill, White, true);
        public static ThingVm Key(Color fill) => new ThingVm(KEY, fill, fill);
        public static ThingVm PacmanGhost => new ThingVm(PACMAN_GHOST, GhostWhite, LightBlue);

        public ThingVm(string path, Color fill, Color stroke, bool shouldRotate = false)
        {
            Geometry = PathGeometry.Parse(path);
            Fill = fill.ToBrush();
            Stroke = stroke.ToBrush();
            ShouldRotate = shouldRotate;
        }
    }
}
