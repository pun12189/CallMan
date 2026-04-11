using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BahiKitab.Helper
{
    public class BoolToColorConverter : IValueConverter
    {
        // Convert: Bool -> Color
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                // Green (#27ae60) for true, Gray (#bdc3c7) for false
                return b ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#27ae60"))
                         : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bdc3c7"));
            }
            return Brushes.Gray;
        }

        // ConvertBack: Not needed for this UI
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
