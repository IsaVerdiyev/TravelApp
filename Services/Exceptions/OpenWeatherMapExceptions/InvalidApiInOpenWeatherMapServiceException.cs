using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Exceptions.OpenWeatherMapExceptions
{
    class InvalidApiInOpenWeatherMapServiceException: Exception
    {
        public string ApiKey { get; set; }
        public InvalidApiInOpenWeatherMapServiceException(string message = "Request was made by invalid api key in openweathermap service", Exception ex = null) : base(message, ex)
        {

        }
    }
}
