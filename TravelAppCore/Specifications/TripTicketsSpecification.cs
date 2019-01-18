using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    class TripTicketsSpecification: BaseSpecification<Ticket>
    {
        public TripTicketsSpecification(Trip trip) : base(t => t.TripId == trip.Id) { }

        public TripTicketsSpecification(int tripId) : base(t => t.TripId == tripId) { }

    }
}
