using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ICityService
    {
        City AddCity(City city);
        Task<City> AddCityAsync(City city);
        City GetCityFromReposByFullname(string fullname);
        Task<City> GetCityFromReposByFullnameAsync(string fullname);
    }
}
