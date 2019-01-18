using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    class TripCitiesSpecification: BaseSpecification<City>
    {
        public TripCitiesSpecification(Trip trip): base(c => c.TripId == trip.Id) { }

        public TripCitiesSpecification(int tripId) : base(c => c.TripId == tripId) { }

    }
}
