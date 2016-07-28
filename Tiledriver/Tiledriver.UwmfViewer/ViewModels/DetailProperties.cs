using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class DetailProperties
    {
        public string Category { get; }
        public string Title { get; }
        public string Value { get; }

        public DetailProperties(string category, string title, string value)
        {
            Category = category;
            Title = title;
            Value = value;
        }
    }
}
