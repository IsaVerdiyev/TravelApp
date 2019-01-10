using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

namespace TravelAppCore.Services
{
    public class CityService : ICityService
    {
        IRepository<City> cityRepository;

        public CityService(IRepository<City> cityRepository)
        {
            this.cityRepository = cityRepository;
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
