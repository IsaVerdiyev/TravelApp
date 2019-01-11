using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ICityMatchesSearcherFromApi<MatchesSearchInputType, MatchesSearchOutputType, MatchType>
    {
        MatchesSearchOutputType GetMatchesFromApiByInput(MatchesSearchInputType input);
        Task<MatchesSearchOutputType> GetMatchesFromApiByInputAsync(MatchesSearchInputType input);

        City GetCityFromApiBySelectedMatch(MatchType match);
        Task<City> GetCityFromApiBySelectedMatchAsync(MatchType match);
    }
}
