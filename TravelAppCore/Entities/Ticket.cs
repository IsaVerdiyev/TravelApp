using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelAppCore.Entities
{
    public class Ticket: BaseEntity
    {
        //[Required]
        [StringLength(60)]
        [DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
