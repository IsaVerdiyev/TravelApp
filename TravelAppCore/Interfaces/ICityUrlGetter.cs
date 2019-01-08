using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppCore.Interfaces
{
    public interface ICityUrlGetter
    {
        string GetCityImageUrl(string cityName);
        Task<string> GetCityImageUrlAsync(string cityName);
    }
}
