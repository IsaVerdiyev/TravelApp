using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ICityFromApiGetter<InputType>
    {
        City GetCityFromApiByName(InputType input);

        Task<City> GetCityFromApiByNameAsync(InputType input);
    }
}
