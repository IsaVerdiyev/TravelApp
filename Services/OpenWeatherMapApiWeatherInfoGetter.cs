using Newtonsoft.Json.Linq;
using Services.Exceptions.OpenWeatherMapExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Extensions;
using TravelAppCore.Interfaces;

namespace Services
{
    public class OpenWeatherMapWeatherInfoGetter : IWeatherInfoGetter
    {
        string firstPartOfRequestCurrent = "http://api.openweathermap.org/data/2.5/weather?&units=metric&q=";
        string firstPartOfRequestForecast = "http://api.openweathermap.org/data/2.5/forecast?&units=metric&q=";
        string secondPartOfRequest = "&APPID=";
        string iconUrlFirstPart = "http://openweathermap.org/img/w/";
        string iconsFolderPath = "OpenWeatherMapIcons//";
        string apiKey;

        public string IconsFolderPath => iconsFolderPath;

        public OpenWeatherMapWeatherInfoGetter(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public Weather GetCurrentWeatherOfCity(string city)
        {
            string response = GetCurrentWeatherResponseFromWeatherOpenMap(city);
            return ParseJsonCurrentWeatherResponseIntoWeather(response);
        }

        public async Task<Weather> GetCurrentWeatherOfCityAsync(string city)
        {
            string response = await GetCurrentWeatherResponseFromWeatherOpenMapAsync(city);
            return ParseJsonCurrentWeatherResponseIntoWeather(response);
        }

        public List<Weather> GetForecastWeathersOfCity(string city)
        {
            string response = GetForecastResponseFromWeatherOpenMap(city);
            return ParseJsonForecastResponseIntoListOfWeathers(response);
        }

        public async Task<List<Weather>> GetForecastWeathersOfCityAsync(string city)
        {
            string response = await GetForecastResponseFromWeatherOpenMapAsync(city);
            return ParseJsonForecastResponseIntoListOfWeathers(response);
        }

        string GetCurrentWeatherResponseFromWeatherOpenMap(string city)
        {
            WebClient client = new WebClient();
            try
            {
                return client.DownloadString($"{ firstPartOfRequestCurrent}{ city}{secondPartOfRequest}{apiKey}");

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                client.Dispose();
            }
        }

        async Task<string> GetCurrentWeatherResponseFromWeatherOpenMapAsync(string city)
        {
            WebClient client = new WebClient();
            try
            {
                return await client.DownloadStringTaskAsync($"{ firstPartOfRequestCurrent}{ city}{secondPartOfRequest}{apiKey}");
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                client.Dispose();
            }
        }

        string GetForecastResponseFromWeatherOpenMap(string city)
        {
            WebClient client = new WebClient();
            try
            {
                return client.DownloadString($"{firstPartOfRequestForecast}{city}{secondPartOfRequest}{apiKey}");
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                client.Dispose();
            }
        }

        async Task<string> GetForecastResponseFromWeatherOpenMapAsync(string city)
        {
            WebClient client = new WebClient();
            try
            {
                return await client.DownloadStringTaskAsync($"{firstPartOfRequestForecast}{city}{secondPartOfRequest}{apiKey}");
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                client.Dispose();
            }
        }

        List<Weather> ParseJsonForecastResponseIntoListOfWeathers(string jsonResponse)
        {
            JObject jObj = JObject.Parse(jsonResponse);

            CheckForExceptions(jObj);


            List<Weather> list = jObj["list"].Select(w => {
                string icon = w.SelectToken("weather[0].icon").Value<string>();
                DownloadIconIfNotExist(icon);
                return new Weather
                {
                    Temperature = w.SelectToken("main.temp").Value<double>(),
                    Date = w.SelectToken("dt_txt").Value<DateTime>(),
                    MaxTemperature = w.SelectToken("main.temp_max").Value<double>(),
                    MinTemperature = w.SelectToken("main.temp_min").Value<double>(),
                    Pressure = w.SelectToken("main.pressure").Value<double>(),
                    Humidity = w.SelectToken("main.humidity").Value<double>(),
                    WindSpeed = w.SelectToken("wind.speed").Value<double>(),
                    Description = w.SelectToken("weather[0].description").Value<string>(),
                    IconPath = $"{IconsFolderPath}{icon}.png"
                };
            }).ToList<Weather>();
            return list;
        }

        Weather ParseJsonCurrentWeatherResponseIntoWeather(string jsonResponse)
        {
            JObject jObj = JObject.Parse(jsonResponse);

            CheckForExceptions(jObj);

            string icon = jObj["weather"][0]["icon"].Value<string>();

            DownloadIconIfNotExist(icon);

            Weather currentWeather = new Weather
            {
                Temperature = jObj["main"]["temp"].Value<double>(),
                Date = DateTimeExtensions.GetDateFromSecondsSince1970(jObj["dt"].Value<int>()),
                Description = jObj["weather"][0]["description"].Value<string>(),
                MaxTemperature = jObj["main"]["temp_max"].Value<double>(),
                MinTemperature = jObj["main"]["temp_min"].Value<double>(),
                Humidity = jObj["main"]["humidity"].Value<double>(),
                Pressure = jObj["main"]["pressure"].Value<double>(),
                WindSpeed = jObj["wind"]["speed"].Value<double>(),
                IconPath = $"{IconsFolderPath}{icon}.png"
            };
            return currentWeather;
        }

        void DownloadIconIfNotExist(string iconName)
        {
            if (!File.Exists($"{ IconsFolderPath}{iconName}.png"))
            {
                if (!Directory.Exists(IconsFolderPath))
                {
                    Directory.CreateDirectory(IconsFolderPath);
                }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileTaskAsync($"{iconUrlFirstPart}{iconName}.png", $"{IconsFolderPath}{iconName}.png");

                }
            }
        }

        void CheckForExceptions(JObject jObject)
        {
            if (jObject["cod"].Value<string>() == "404" /*&& jObj["message"].Value<string>() == "city not found"*/)
            {
                throw new CityNotFoundByWeatherInfoGetterServiceException();
            }
            else if (jObject["cod"].Value<string>() == "429")
            {
                throw new RequestLimitationExcedeedInOpenWeatherMapServiceException();
            }
            else if (jObject["cod"].Value<string>() == "401")
            {
                throw new InvalidApiInOpenWeatherMapServiceException($"Request to openweathermap was made by invalid api key: {apiKey}");
            }
        }

    }

}
