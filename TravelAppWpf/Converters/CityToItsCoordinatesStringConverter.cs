using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TravelAppCore.Entities;

namespace TravelAppWpf.Converters
{
    class CityToItsCoordinatesStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            City city = value as City;
            return $"{city?.CityCoordinate.Latitude.ToString()}, {city?.CityCoordinate.Longitude.ToString()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
