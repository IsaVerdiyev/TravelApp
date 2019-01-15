using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelAppCore.Entities
{
    public class City: BaseEntity
    {
        
        public string Name { get; set; }

       
        public string FullName { get; set; }

        
        public string PictureUrl { get; set; }

        public CityCoordinate CityCoordinate { get; set; }

        
        public string Currency { get; set; }

        
        public string Language { get; set; }

        
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
