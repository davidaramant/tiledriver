// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Tiledriver.Gui.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            int i;
            if (str != null 
                && str.StartsWith("#") 
                && int.TryParse(str.Replace("#", ""), NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out i))
            {
                return new BrushConverter().ConvertFrom(str);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
