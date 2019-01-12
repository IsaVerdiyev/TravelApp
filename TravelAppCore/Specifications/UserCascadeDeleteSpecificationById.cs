using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    public class UserCascadeDeleteSpecificationById: BaseSpecification<User>
    {
        public UserCascadeDeleteSpecificationById(int id): base(u => u.Id == id)
        {
            Includes.Add(u => u.Trips.Select(trip => trip.CheckList));
            Includes.Add(u => u.Trips.Select(trip => trip.Cities));
            Includes.Add(u => u.Trips.Select(trip => trip.Tickets));
            
        }
    }
}
