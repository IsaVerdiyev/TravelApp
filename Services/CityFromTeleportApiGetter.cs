using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Exceptions;
using TravelAppCore.Interfaces;

namespace Services
{
    public class CityFromTeleportApiGetter : ICityFromApiGetter<string>, ICityMatchesSearcherFromApi<string, IList<(string cityFullName, string cityUrl)>, string>
    {
        string apiUrl = @"https://api.teleport.org/api/";

        public City GetCityFromApiByName(string cityName)
        {
            string urlOfCity = GetApiUrlOfCityBySearchedCityName(cityName);

            return GetCityFromCityUrl(urlOfCity);

        }

        public async Task<City> GetCityFromApiByNameAsync(string searchedCityName)
        {
            string urlOfCity = await GetApiUrlOfCityBySearchCityNameAsync(searchedCityName);
            return await GetCityFromCityUrlAsync(urlOfCity);
        }

        private City GetCityFromCityUrl(string urlOfCity)
        {
            string response = GetResponseFromCityUrl(urlOfCity);

            JObject jObject = JObject.Parse(response);

            string name = GetNameFromCityResponse(jObject);

            string fullName = GetFullNameFromCityResponse(jObject);

            CityCoordinate cityCoordinate = GetCityCoordinateFromCityResponse(jObject);

            City city = new City
            {
                CityCoordinate = cityCoordinate,
                FullName = fullName,
                Name = name
            };

            try
            {
                string urbanAreaLink = GetUrbanAreaLinkFromCityUrlResponse(jObject);
                string urbanAreaResponse = GetResponseFromUrbanAreaUrl(urbanAreaLink);
                city.PictureUrl = GetImageUrlFromUrbanAreaResponse(urbanAreaResponse);
                return GetCityWithUrbanDetails(urbanAreaLink + "details", city);
            }
            catch (InvalidOperationException ex)
            {
                return GetCityWithoutUrbanDetails(jObject, city);
            }
        }

        private async Task<City> GetCityFromCityUrlAsync(string urlOfCity)
        {
            string response = await GetResponseFromCityUrlAsync(urlOfCity);

            JObject jObject = JObject.Parse(response);
            try
            {
                string urbanAreaLink = GetUrbanAreaLinkFromCityUrlResponse(jObject);
                Task<string> urbanAreaUrlResponseGettingTask = GetResponseFromUrbanAreaUrlAsync(urbanAreaLink);
                Task<(string currency, string language)> urbanDetailsGettingTask = GetCityDetailsFromUrbanDetailsApiAsync(urbanAreaLink + "details");
                Task<string> pictureUrlGettingTask = urbanAreaUrlResponseGettingTask.ContinueWith(t => GetImageUrlFromUrbanAreaResponse(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

                return new City
                {
                    Name = GetNameFromCityResponse(jObject),
                    FullName = GetFullNameFromCityResponse(jObject),
                    CityCoordinate = GetCityCoordinateFromCityResponse(jObject),
                    Language = (await urbanDetailsGettingTask).language,
                    Currency = (await urbanDetailsGettingTask).currency,
                    PictureUrl = await pictureUrlGettingTask
                };
            }
            catch (InvalidOperationException ex)
            {
                Task<string> currencyCodeGettingTaskWhenNoUrbanDetailsAvailable = GetCurrencyCodeWhenNoUrbanDetailsAvailableAsync(jObject);
                return new City
                {
                    Name = GetNameFromCityResponse(jObject),
                    FullName = GetFullNameFromCityResponse(jObject),
                    CityCoordinate = GetCityCoordinateFromCityResponse(jObject),
                    Currency = await currencyCodeGettingTaskWhenNoUrbanDetailsAvailable
                };
            }
        }

        private City GetCityWithUrbanDetails(string urbanAreaDetailsLink, City city)
        {
            string urbanDetailsResponse = GetResponseFromUrbanDetailsLink(urbanAreaDetailsLink);
            JObject jObject = JObject.Parse(urbanDetailsResponse);
            city.Currency = GetCurrencyFromUrbanAreaDetailsResponse(jObject);
            city.Language = GetLanguageFromUrbanAreaDetailsResponse(jObject);
            return city;
        }


        private async Task<(string currency, string language)> GetCityDetailsFromUrbanDetailsApiAsync(string urbanAreaDetailsLink)
        {
            string urbanDetailsResponse = await GetResponseFromUrbanAreaUrlAsync(urbanAreaDetailsLink);
            JObject jObject = JObject.Parse(urbanDetailsResponse);
            string currency = GetCurrencyFromUrbanAreaDetailsResponse(jObject);
            string language = GetLanguageFromUrbanAreaDetailsResponse(jObject);
            return (currency, language);
        }

        private string GetImageUrlFromUrbanAreaResponse(string response)
        {
            JObject jObject = JObject.Parse(response);

            return jObject["_links"]["ua:images"]["href"].Value<string>();
        }

        private string GetResponseFromUrbanAreaUrl(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(url);
            }
        }

        private async Task<string> GetResponseFromUrbanAreaUrlAsync(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync(url);
            }
        }

        private string GetLanguageFromUrbanAreaDetailsResponse(JObject jObject)
        {
            return jObject["categories"]
                .First(c => c["id"].Value<string>() == "LANGUAGE")["data"]
                .First(d => d["id"].Value<string>() == "SPOKEN-LANGUAGES")
                ["string_value"].Value<string>();
        }

        private string GetCurrencyFromUrbanAreaDetailsResponse(JObject jObject)
        {
            return jObject["categories"]
                .First(c => c["id"].Value<string>() == "ECONOMY")["data"]
                .First(d => d["id"].Value<string>() == "CURRENCY-URBAN-AREA")
                ["string_value"].Value<string>();
        }

        private string GetResponseFromUrbanDetailsLink(string urbanDetailsLink)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(urbanDetailsLink);
            }
        }

        private async Task<string> GetResponseFromUrbanDetailsLinkAsync(string urbanDetailsLink)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync(urbanDetailsLink);
            }
        }

        private City GetCityWithoutUrbanDetails(JObject jObject, City city)
        {
            string countryUrl = GetCountryUrlFromCityResponse(jObject);
            string countryResponse = GetResponseFromCountryUrl(countryUrl);
            string currencyCode = GetCurrencyCodeFromCountryResponse(countryResponse);

            city.Currency = currencyCode;

            return city;
        }

        private async Task<string> GetCurrencyCodeWhenNoUrbanDetailsAvailableAsync(JObject jObject)
        {
            string countryUrl = GetCountryUrlFromCityResponse(jObject);
            string countryResponse = await GetResponseFromCountryUrlAsync(countryUrl);
            return GetCurrencyCodeFromCountryResponse(countryResponse);
        }

        private string GetCurrencyCodeFromCountryResponse(string countryResponse)
        {
            JObject jObject = JObject.Parse(countryResponse);

            return jObject["currency_code"].Value<string>();
        }

        private string GetResponseFromCountryUrl(string countryUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(countryUrl);
            }
        }

        private async Task<string> GetResponseFromCountryUrlAsync(string countryUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync(countryUrl);
            }
        }

        private string GetCountryUrlFromCityResponse(JObject jObject)
        {
            return jObject["_links"]["city:country"]["href"].Value<string>();
        }

        private string GetUrbanAreaLinkFromCityUrlResponse(JObject jObject)
        {
            return jObject["_links"]["city:urban_area"]["href"].Value<string>();
        }

        private string GetNameFromCityResponse(JObject jObject)
        {
            return jObject["name"].Value<string>();
        }

        private string GetFullNameFromCityResponse(JObject jObject)
        {
            return jObject["full_name"].Value<string>();
        }

        private CityCoordinate GetCityCoordinateFromCityResponse(JObject jObject)
        {
            float latitude = jObject["location"]["latlon"]["latitude"].Value<float>();
            float longitude = jObject["location"]["latlon"]["longitude"].Value<float>();
            return new CityCoordinate { Latitude = latitude, Longitude = longitude };
        }

        private string GetResponseFromCityUrl(string cityUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(cityUrl);
            }
        }

        private async Task<string> GetResponseFromCityUrlAsync(string cityUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync(cityUrl);
            }
        }

        private string GetApiUrlOfCityBySearchedCityName(string searchedCityName)
        {
            string responseFromApi = GetResponseOfSearchByCityNameFromApi(searchedCityName);
            return GetUrlOfFoundCityFromReturnedResponse(searchedCityName, responseFromApi);
        }

        private async Task<string> GetApiUrlOfCityBySearchCityNameAsync(string searchedCityName)
        {
            string responseFromApi = await GetResponseOfSearchByCityNameFromApiAsync(searchedCityName);
            return GetUrlOfFoundCityFromReturnedResponse(searchedCityName, responseFromApi);
        }

        private string GetUrlOfFoundCityFromReturnedResponse(string searchedCity, string response)
        {
            JObject jObject = JObject.Parse(response);

            try
            {
                return jObject["_embedded"]["city:search-results"].First(jt => jt["matching_alternate_names"].Any(jtn => string.Compare(jtn["name"].Value<String>(), searchedCity, true) == 0))["_links"]["city:item"]["href"].Value<string>();
            }
            catch (InvalidOperationException ex)
            {
                try
                {
                    return jObject["_embedded"]["city:search-results"].First(jt => string.Compare(jt["matching_full_name"].Value<string>(), searchedCity, true) == 0)["_links"]["city:item"]["href"].Value<string>();
                }
                catch (InvalidOperationException innerEx)
                {
                    throw new CityNotFoundByApiWithSpecifiedNameException($"City with name {searchedCity} was not found by api", innerEx);
                }
            }
        }

        private async Task<string> GetResponseOfSearchByCityNameFromApiAsync(string searchedCityName)
        {
            string apiForCitySearch = $"{apiUrl}/cities/?search={searchedCityName}";

            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync(apiForCitySearch);
            }
        }

        private string GetResponseOfSearchByCityNameFromApi(string name)
        {
            string apiForCitySearch = $"{apiUrl}/cities/?search={name}";
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(apiForCitySearch);
            }

        }

        public IList<(string cityFullName, string cityUrl)> GetMatchesFromApiByInput(string cityName)
        {

            string citiesResponse = GetResponseOfSearchByCityNameFromApi(cityName);

            return GetListOfCitiesFromSearchByCityNameResponse(cityName, citiesResponse);
        }

        public async Task<IList<(string cityFullName, string cityUrl)>> GetMatchesFromApiByInputAsync(string cityName)
        {
            string citiesResponse = await GetResponseOfSearchByCityNameFromApiAsync(cityName);

            return GetListOfCitiesFromSearchByCityNameResponse(cityName, citiesResponse);
        }

        private IList<(string fullname, string cityUrl)> GetListOfCitiesFromSearchByCityNameResponse(string cityName, string response)
        {
            JObject jObject = JObject.Parse(response);
            try
            {
                return jObject["_embedded"]["city:search-results"].Select(r =>
                (
                    fullname: r["matching_full_name"].Value<string>(),
                    cityUrl: r["_links"]["city:item"]["href"].Value<string>()
                )).ToList();
            }
            catch (InvalidOperationException ex)
            {
                throw new CityNotFoundByApiWithSpecifiedNameException($"No city was found with name {cityName}", ex);
            }
        }


        public City GetCityFromApiBySelectedMatch(string SelectedMatchCityUrl)
        {
            return GetCityFromCityUrl(SelectedMatchCityUrl);
        }

        public async Task<City> GetCityFromApiBySelectedMatchAsync(string SelectedMatchCityUrl)
        {
            return await GetCityFromCityUrlAsync(SelectedMatchCityUrl);
        }
    }
}
