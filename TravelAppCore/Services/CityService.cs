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
        private readonly IRepository<City> repository;

        public CityService(IRepository<City> repository)
        {
            this.repository = repository;
        }

        public City AddCity(City city)
        {
            return repository.Add(city);
        }

        public async Task<City> AddCityAsync(City city)
        {
            return await repository.AddAsync(city);
        }

        public City GetCityFromReposByFullname(string fullname)
        {
            return repository.GetSingleBySpec(new CityByFullNameSpecification(fullname));
        }

        public async Task<City> GetCityFromReposByFullnameAsync(string fullname)
        {
            return await repository.GetSingleBySpecAsync(new CityByFullNameSpecification(fullname));
        }
    }
}
