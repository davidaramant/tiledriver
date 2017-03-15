// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Globalization;
using System.Windows.Data;
using static System.String;
using static System.Windows.Visibility;

namespace Tiledriver.Gui.Converters
{
    public class EmptyStringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return IsNullOrWhiteSpace(str) ? Collapsed : Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
