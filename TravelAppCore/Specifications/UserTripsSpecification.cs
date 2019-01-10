using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

namespace TravelAppCore.Specifications
{
    class UserTripsSpecification : BaseSpecification<Trip>
    {
        public UserTripsSpecification(int UserId): base(t => t.UserId == UserId) { }
       

        public UserTripsSpecification(User user): base(t => t.UserId == user.Id) { }
    }
}
