using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

namespace TravelAppCore.Services
{
    public class CityCoordinateGetter : ICityCoordinateGetter
    {
        private readonly IRepository<CityCoordinate> repository;

        public CityCoordinateGetter(IRepository<CityCoordinate> repository)
        {
            this.repository = repository;
        }

        public CityCoordinate GetCityCoordinateOfCity(City city)
        {
            return repository.GetById(city.Id);
        }

        public async Task<CityCoordinate> GetCityCoordinateOfCityAsync(City city)
        {
            return await repository.GetByIdAsync(city.Id);
        }
    }
}
