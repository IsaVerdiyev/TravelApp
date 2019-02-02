using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    class TripCitiesSpecification: BaseSpecification<DestinationCityInTrip>
    {
        public TripCitiesSpecification(Trip trip): base(d => d.TripId == trip.Id) {
            Includes.Add(d => d.DestinationCity);
        }

        public TripCitiesSpecification(int tripId) : base(d => d.TripId == tripId) {
            Includes.Add(d => d.DestinationCity);
        }

    }
}
