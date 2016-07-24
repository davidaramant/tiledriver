using System;
using System.Linq;
using System.Windows.Controls;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class DetailView
    {
        public DetailView()
        {
            InitializeComponent();
        }

        internal void Update(object sender, MapItemEventArgs e)
        {
            Panel.Children.Clear();

            // ITEM
            foreach (var mapItem in e.MapItems)
            {
                var itemPanel = new StackPanel();
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

        private static Label TitleLabel(string title)
        {
            return new Label()
            {
                Content = title,
            };
        }

        private static Label ValueLabel(string value)
        {
            return new Label()
            {
                Content = value,
            };
        }

        private static Label ItemHeader(string detailType)
        {
            return new Label()
            {
                Content = detailType,
            };
        }

        private static Label CategoryHeader(string categoryKey)
        {
            return new Label()
            {
                Content = categoryKey,
            };
        }
    }
}
