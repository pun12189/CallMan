using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BahiKitab.Core
{
    // ==============================================================================
    // 3. Converter: Converts Lead ID to "Create" or "Update" button text
    // ==============================================================================
    public class IdToButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && id > 0)
            {
                return "Update Lead";
            }
            return "Create New Lead";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
