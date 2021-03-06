﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelAppCore.Entities
{
    public class ToDoItem: BaseEntity
    {
        
        public string Name { get; set; }

        public bool Done { get; set; }

       
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
