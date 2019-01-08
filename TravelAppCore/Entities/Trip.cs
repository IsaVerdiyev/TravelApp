using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelAppCore.Entities
{
    public class Trip: BaseEntity
    {
        public IEnumerable<City> Cities { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DepartureTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ArrivalTime { get; set; }

        
        public IEnumerable<CheckItem> CheckList { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }

    }
}
