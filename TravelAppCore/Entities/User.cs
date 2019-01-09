using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelAppCore.Entities
{
    public class User: BaseEntity
    {
        //[Required]
        [StringLength(50)]
        public string NickName { get; set; }

        //[Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        public ICollection<Trip> Trips { get; set; }
       
    }
}
