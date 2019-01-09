using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

namespace TravelAppCore.Services
{
    public class TripService : ITripService
    {
        IRepository<City> cityRepository;

        IRepository<Trip> tripRepository;

        public TripService(IRepository<City> cityRepository, IRepository<Trip> tripRepository)
        {
            this.cityRepository = cityRepository;
            this.tripRepository = tripRepository;
        }


        public City AddCity(Trip trip, City city)
        {
            city.TripId = trip.Id;
            return cityRepository.Add(city);
        }

        public async Task<City> AddCityAsync(Trip trip, City city)
        {
            city.TripId = trip.Id;
            return await cityRepository.AddAsync(city);
        }

        public void ChangeArivalDate(Trip trip, DateTime arrivalDate)
        {
            trip.ArriavalDate = arrivalDate;
            tripRepository.Update(trip);
        }

        public async Task ChangeArrivalDateAsync(Trip trip, DateTime arrivalDate)
        {
            trip.ArriavalDate = arrivalDate;
            await tripRepository.UpdateAsync(trip);
        }

        public void ChangeDepartureDate(Trip trip, DateTime departureDate)
        {
            trip.DepartureDate = departureDate;
            tripRepository.Update(trip);
        }

        public async Task ChangeDepartureDateAsync(Trip trip, DateTime departureDate)
        {
            trip.DepartureDate = departureDate;
            await tripRepository.UpdateAsync(trip);
        }

        public void RemoveCity(City city)
        {
            cityRepository.Delete(city);
        }

        public async Task RemoveCityAsync(City city)
        {
            await cityRepository.DeleteAsync(city);
        }
    }
}
