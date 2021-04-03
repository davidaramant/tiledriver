// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Gui.ViewModels
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
