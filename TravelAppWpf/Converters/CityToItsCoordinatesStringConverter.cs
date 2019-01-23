using Microsoft.Maps.MapControl.WPF;
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
            if(city == null)
            {
                return null;
            }
            return new Location(System.Convert.ToDouble(city.CityCoordinate.Latitude, CultureInfo.InvariantCulture), System.Convert.ToDouble(city.CityCoordinate.Longitude, CultureInfo.InvariantCulture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
