using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelAppCore.Entities
{
    public class User: BaseEntity
    {
       
        public string NickName { get; set; }

        
        public string Password { get; set; }

        
        public string Email { get; set; }


        public ICollection<Trip> Trips { get; set; }
       
    }
}
