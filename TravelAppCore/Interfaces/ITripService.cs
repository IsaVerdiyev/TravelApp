using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ITripService
    {
        City AddCity(Trip trip, City city);
        Task<City> AddCityAsync(Trip trip, City city);

        void RemoveCity(Trip trip, City city);
        Task RemoveCityAsync(Trip trip, City city);

        void ChangeArivalDate(Trip trip, DateTime arrivalDate);
        Task ChangeArrivalDateAsync(Trip trip, DateTime arrivalDate);

        void ChangeDepartureDate(Trip trip, DateTime departureDate);
        Task ChangeDepartureDateAsync(Trip trip, DateTime departureDate);
    }
}
