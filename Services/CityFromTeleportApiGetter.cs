using Newtonsoft.Json.Linq;
using Services.Exceptions.CityInfoFromTeleportApiGetterServiceExceptions;
using Services.Exceptions.OpenWeatherMapExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TravelAppCore.Entities;
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


        public City GetCityFromApiBySelectedMatch(string SelectedMatchCityUrl)
        {
            return GetCityFromCityUrl(SelectedMatchCityUrl);
        }

        public async Task<City> GetCityFromApiBySelectedMatchAsync(string SelectedMatchCityUrl)
        {
            return await GetCityFromCityUrlAsync(SelectedMatchCityUrl);
        }

        public IList<(string cityFullName, string cityUrl)> GetMatchesFromApiByInput(string cityName)
        {

            string citiesResponse = LoadResponseFromSomeLink($"{apiUrl}/cities/?search={cityName}");

            return GetListOfCitiesFromSearchByCityNameResponse(cityName, citiesResponse);
        }

        public async Task<IList<(string cityFullName, string cityUrl)>> GetMatchesFromApiByInputAsync(string cityName)
        {
            string citiesResponse = await LoadResponseFromSomeLinkAsync($"{apiUrl}/cities/?search={cityName}");

            return GetListOfCitiesFromSearchByCityNameResponse(cityName, citiesResponse);
        }



        private string LoadResponseFromSomeLink(string link)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(link);
            }
        }

        private async Task<string> LoadResponseFromSomeLinkAsync(string link)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync(link);
            }
        }





        private City GetCityFromCityUrl(string urlOfCity)
        {
            string response = LoadResponseFromSomeLink(urlOfCity);

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
                string urbanAreaResponse = LoadResponseFromSomeLink(urbanAreaLink);
                (string currency, string language) urbanDetails = GetCityDetailsFromUrbanDetailsApi(urbanAreaLink + "details");
                string pictureUrlLink = GetImagesLinkFromUrbanAreaResponse(urbanAreaResponse);
                string pictureUrlLinkResponse = LoadResponseFromSomeLink(pictureUrlLink);
                string pictureUrl = GetImageUrlFromImageLinkResponse(pictureUrlLinkResponse);
                return new City
                {
                    Name = GetNameFromCityResponse(jObject),
                    FullName = GetFullNameFromCityResponse(jObject),
                    CityCoordinate = GetCityCoordinateFromCityResponse(jObject),
                    Language = urbanDetails.language,
                    Currency = urbanDetails.currency,
                    PictureUrl = pictureUrl
                };
            }
            catch (InvalidOperationException ex)
            {
                return GetCityWithoutUrbanDetails(jObject, city);
            }
        }

        private async Task<City> GetCityFromCityUrlAsync(string urlOfCity)
        {
            string response = await LoadResponseFromSomeLinkAsync(urlOfCity);

            JObject jObject = JObject.Parse(response);
            try
            {
                string urbanAreaLink = GetUrbanAreaLinkFromCityUrlResponse(jObject);
                Task<string> urbanAreaUrlResponseGettingTask = LoadResponseFromSomeLinkAsync(urbanAreaLink);
                Task<(string currency, string language)> urbanDetailsGettingTask = GetCityDetailsFromUrbanDetailsApiAsync(urbanAreaLink + "details");
                Task<string> pictureUrlLinkGettingTask = urbanAreaUrlResponseGettingTask.ContinueWith(t => GetImagesLinkFromUrbanAreaResponse(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
                Task<string> pictureUrlLinkResponseGettingTask = pictureUrlLinkGettingTask.ContinueWith(t => LoadResponseFromSomeLink(t.Result));

                return new City
                {
                    Name = GetNameFromCityResponse(jObject),
                    FullName = GetFullNameFromCityResponse(jObject),
                    CityCoordinate = GetCityCoordinateFromCityResponse(jObject),
                    Language = (await urbanDetailsGettingTask).language,
                    Currency = (await urbanDetailsGettingTask).currency,
                    PictureUrl = GetImageUrlFromImageLinkResponse(await pictureUrlLinkResponseGettingTask)
                };
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
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
                else
                {
                    throw;
                }
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

  
        private (string currency, string language) GetCityDetailsFromUrbanDetailsApi(string urbanAreaDetailsLink)
        {
            string urbanDetailsResponse = LoadResponseFromSomeLink(urbanAreaDetailsLink);
            JObject jObject = JObject.Parse(urbanDetailsResponse);
            string currency = GetCurrencyFromUrbanAreaDetailsResponse(jObject);
            string language = GetLanguageFromUrbanAreaDetailsResponse(jObject);
            return (currency, language);
        }

        private async Task<(string currency, string language)> GetCityDetailsFromUrbanDetailsApiAsync(string urbanAreaDetailsLink)
        {
            string urbanDetailsResponse = await LoadResponseFromSomeLinkAsync(urbanAreaDetailsLink);
            JObject jObject = JObject.Parse(urbanDetailsResponse);
            string currency = GetCurrencyFromUrbanAreaDetailsResponse(jObject);
            string language = GetLanguageFromUrbanAreaDetailsResponse(jObject);
            return (currency, language);
        }


        private string GetImagesLinkFromUrbanAreaResponse(string response)
        {
            JObject jObject = JObject.Parse(response);

            return jObject["_links"]["ua:images"]["href"].Value<string>();
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


        private City GetCityWithoutUrbanDetails(JObject jObject, City city)
        {
            string countryUrl = GetCountryUrlFromCityResponse(jObject);
            string countryResponse = LoadResponseFromSomeLink(countryUrl);
            string currencyCode = GetCurrencyCodeFromCountryResponse(countryResponse);

            city.Currency = currencyCode;

            return city;
        }

        private async Task<string> GetCurrencyCodeWhenNoUrbanDetailsAvailableAsync(JObject jObject)
        {
            string countryUrl = GetCountryUrlFromCityResponse(jObject);
            string countryResponse = await LoadResponseFromSomeLinkAsync(countryUrl);
            return GetCurrencyCodeFromCountryResponse(countryResponse);
        }

        private string GetCurrencyCodeFromCountryResponse(string countryResponse)
        {
            JObject jObject = JObject.Parse(countryResponse);

            return jObject["currency_code"].Value<string>();
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


        private string GetApiUrlOfCityBySearchedCityName(string searchedCityName)
        {
            string responseFromApi = LoadResponseFromSomeLink($"{apiUrl}/cities/?search={searchedCityName}");
            return GetUrlOfFoundCityFromReturnedResponse(searchedCityName, responseFromApi);
        }

        private async Task<string> GetApiUrlOfCityBySearchCityNameAsync(string searchedCityName)
        {
            string responseFromApi = await LoadResponseFromSomeLinkAsync($"{apiUrl}/cities/?search={searchedCityName}");
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
                    throw new CityNotFoundByTeleportApiWithSpecifiedNameException($"City with name {searchedCity} was not found by api", innerEx);
                }
            }
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
                throw new InvalidApiInOpenWeatherMapServiceException($"No city was found with name {cityName}", ex);
            }
        }

        private string GetImageUrlFromImageLinkResponse(string response)
        {
            JObject jObject = JObject.Parse(response);

            return jObject["photos"][0]["image"]["web"].Value<string>();
        }


    }
}
