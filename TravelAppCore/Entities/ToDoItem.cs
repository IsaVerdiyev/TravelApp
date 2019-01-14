using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelAppCore.Entities
{
    public class ToDoItem: BaseEntity
    {
        //[StringLength(50)]
        //[Required]
        public string Name { get; set; }

        public bool Done { get; set; }

        //[ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
