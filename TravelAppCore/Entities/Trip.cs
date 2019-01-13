using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelAppCore.Entities
{
    public class Trip: BaseEntity
    {
        public ICollection<City> Cities { get; set; }

        //[Required]
        //[DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        //[Required]
        //[DataType(DataType.Date)]
        public DateTime ArriavalDate { get; set; }

        
        public ICollection<ToDoItem> CheckList { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        //[ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
