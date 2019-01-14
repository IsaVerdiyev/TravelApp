using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Exceptions.OpenWeatherMapExceptions
{
    class RequestLimitationExcedeedInOpenWeatherMapServiceException: Exception
    {
        public RequestLimitationExcedeedInOpenWeatherMapServiceException(string message = "Request limitation was excedeed"): base("message")
        {

        }
    }
}
