using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface IWeatherInfoGetter
    {
        Weather GetCurrentWeatherOfCity(string city);
        Task<Weather> GetCurrentWeatherOfCityAsync(string city);
        List<Weather> GetForecastWeathersOfCity(string city);
        Task<List<Weather>> GetForecastWeathersOfCityAsync(string city);
        string IconsFolderPath { get; }
    }
}
