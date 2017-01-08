// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Metadata
{
    public class NamedItem
    {
        public NamedItem(string formatName, string className)
        {
            FormatName = formatName;
            ClassName = className;
        }

        public string FormatName { get; }
        public string ClassName { get; }
    }
}