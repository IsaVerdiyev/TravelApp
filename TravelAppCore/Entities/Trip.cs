using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelAppCore.Entities
{
    public class Trip: BaseEntity
    {

        public string Name { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArriavalDate { get; set; }

        public ICollection<ToDoItem> CheckList { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<DestinationCityInTrip> Destinations { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
