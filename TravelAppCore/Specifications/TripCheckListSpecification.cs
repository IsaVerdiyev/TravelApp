using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    class TripCheckListSpecification: BaseSpecification<ToDoItem>
    {
        public TripCheckListSpecification(Trip trip) : base(t => t.TripId == trip.Id) { }

        public TripCheckListSpecification(int tripId) : base(t => t.TripId == tripId) { }

    }
}
