using System;
using System.Collections.Generic;
using System.Text;

namespace TravelAppCore.Entities
{
    public class CityCoordinate: BaseEntity
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
