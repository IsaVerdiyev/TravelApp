using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Specifications;

namespace TravelAppCore.Interfaces
{
    public interface ICityService
    {
        City AddCity(Trip trip, City city);
        Task<City> AddCityAsync(Trip trip, City city);

        void RemoveCity(DeleteByIdSpecification<City> deleteByIdSpecification);
        Task RemoveCityAsync(DeleteByIdSpecification<City> deleteByIdSpecification);

        IReadOnlyList<City> GetCitiesOfTrip(Trip trip);
        Task<IReadOnlyList<City>> GetCitiesOfTripAsync(Trip trip);
    }
}
