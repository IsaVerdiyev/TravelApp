using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    interface ICityCoordinatesGetter
    {
        CityCoordinate GetCityCoordinate(string cityName);
        Task<CityCoordinate> GetCityCoordinateAsync(string cityName);
    }
}
