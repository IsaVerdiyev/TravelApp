using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelAppCore.Entities
{
    public class City: BaseEntity
    {
        //[Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string PictureUrl { get; set; }

        public CityCoordinate CityCoordinate { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        [StringLength(20)]
        public string Language { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
