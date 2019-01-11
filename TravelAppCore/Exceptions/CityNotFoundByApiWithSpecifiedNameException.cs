using System;
using System.Collections.Generic;
using System.Text;

namespace TravelAppCore.Exceptions
{
    public class CityNotFoundByApiWithSpecifiedNameException: Exception
    {
        public CityNotFoundByApiWithSpecifiedNameException(string message): base(message)
        {

        }

        public CityNotFoundByApiWithSpecifiedNameException(Exception ex): base("City with specified name not found", ex)
        {

        }

        public CityNotFoundByApiWithSpecifiedNameException(string message, Exception ex): base(message, ex)
        {

        }
    }
}
