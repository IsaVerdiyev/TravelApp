using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ICityCoordinateGetter
    {
        CityCoordinate GetCityCoordinateOfCity(City city);
        Task<CityCoordinate> GetCityCoordinateOfCityAsync(City city);
    }
}
