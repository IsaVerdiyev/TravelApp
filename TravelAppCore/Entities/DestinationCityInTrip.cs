using System;
using System.Collections.Generic;
using System.Text;

namespace TravelAppCore.Entities
{
    public class DestinationCityInTrip: BaseEntity
    {

        public int OrderNumber { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
       
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
