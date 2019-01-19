using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppWpf.Messages
{
    class AddCityViewModelMessage
    {
        public User User { get; set; }
        public Trip Trip { get; set; }
    }
}
