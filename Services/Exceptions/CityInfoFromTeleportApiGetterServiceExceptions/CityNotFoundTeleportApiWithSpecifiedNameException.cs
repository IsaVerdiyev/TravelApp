using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Exceptions.CityInfoFromTeleportApiGetterServiceExceptions
{
    public class CityNotFoundByTeleportApiWithSpecifiedNameException: Exception
    {
        public CityNotFoundByTeleportApiWithSpecifiedNameException(string message): base(message)
        {

        }

        public CityNotFoundByTeleportApiWithSpecifiedNameException(Exception ex): base("City with specified name not found", ex)
        {

        }

        public CityNotFoundByTeleportApiWithSpecifiedNameException(string message, Exception ex): base(message, ex)
        {

        }
    }
}
