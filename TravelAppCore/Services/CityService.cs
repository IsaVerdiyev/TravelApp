using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;

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

        public IReadOnlyList<City> GetCitiesOfTrip(Trip trip)
        {
            return cityRepository.List(new TripCitiesSpecification(trip));
        }

        public async Task<IReadOnlyList<City>> GetCitiesOfTripAsync(Trip trip)
        {
            return await cityRepository.ListAsync(new TripCitiesSpecification(trip));
        }

        public void RemoveCity(DeleteByIdSpecification<City> specification)
        {
            cityRepository.DeleteBySpec(specification);
        }

        public async Task RemoveCityAsync(DeleteByIdSpecification<City> specification)
        {
            await cityRepository.DeleteBySpecAsync(specification);
        }
    }
}
