using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ICityService
    {
        City AddCity(Trip trip, City city);
        Task<City> AddCityAsync(Trip trip, City city);

        void RemoveCity(City city);
        Task RemoveCityAsync(City city);
    }
}
