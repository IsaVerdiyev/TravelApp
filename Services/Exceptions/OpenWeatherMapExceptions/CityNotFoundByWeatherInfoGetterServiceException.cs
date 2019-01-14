using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Exceptions.OpenWeatherMapExceptions
{
    class CityNotFoundByWeatherInfoGetterServiceException: Exception
    {
        public CityNotFoundByWeatherInfoGetterServiceException(Exception ex = null): base("City was not found by open weather map api", ex) 
        {

        }

        public CityNotFoundByWeatherInfoGetterServiceException(string message, Exception ex = null): base(message, ex)
        {

        }
    }
}
