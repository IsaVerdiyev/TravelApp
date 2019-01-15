using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelAppCore.Entities
{
    public class Ticket: BaseEntity
    {
        
        public string ImagePath { get; set; }

        
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
