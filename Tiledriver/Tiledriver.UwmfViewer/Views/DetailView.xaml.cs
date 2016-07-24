using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Media.Colors;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class DetailView
    {
        public DetailView()
        {
            InitializeComponent();
            Panel.Margin = new Thickness(4);
        }

        internal void Update(object sender, MapItemEventArgs e)
        {
            Panel.Children.Clear();

            // ITEM
            foreach (var mapItem in e.MapItems)
            {
                var itemPanel = new StackPanel()
                {
                    Margin = new Thickness(0, 0, 0, 16),
                };
                itemPanel.Children.Add(ItemHeader(mapItem.DetailType));

                // CATEGORY
                foreach (var category in mapItem.Details.GroupBy(d => d.Category))
                {
                    var categoryPanel = new StackPanel();
                    if (!string.IsNullOrWhiteSpace(category.Key))
                    {
                        categoryPanel.Children.Add(CategoryHeader(category.Key));
                    }

                    // DETAIL KEY/VALUE
                    foreach (var detail in category)
                    {
                        var detailPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        detailPanel.Children.Add(TitleLabel(detail.Title));
                        detailPanel.Children.Add(ValueLabel(detail.Value));
                        categoryPanel.Children.Add(detailPanel);
                    }

                    itemPanel.Children.Add(categoryPanel);
                }

                Panel.Children.Add(itemPanel);
            }
        }

        private static Label ItemHeader(string detailType)
        {
            return new Label()
            {
                Content = detailType,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Blue.ToBrush(),
                BorderBrush = LightGray.ToBrush(),
                BorderThickness = new Thickness(0, 0, 0, 0.5)
            };
        }

        private static Label CategoryHeader(string categoryKey)
        {
            return new Label()
            {
                Content = categoryKey,
                FontWeight = FontWeights.Bold,
                Foreground = Color.FromRgb(64,64,64).ToBrush(),
            };
        }

        private static Label TitleLabel(string title)
        {
            return new Label()
            {
                Content = title,
                Margin = new Thickness(16, 0, 0, 0),
                Width = 96,
            };
        }

        private static Label ValueLabel(string value)
        {
            return new Label()
            {
                Content = value,
            };
        }
    }
}
