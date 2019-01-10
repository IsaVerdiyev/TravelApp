using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ITripService
    {

        void ChangeArivalDate(Trip trip, DateTime arrivalDate);
        Task ChangeArrivalDateAsync(Trip trip, DateTime arrivalDate);

        void ChangeDepartureDate(Trip trip, DateTime departureDate);
        Task ChangeDepartureDateAsync(Trip trip, DateTime departureDate);

        Trip AddTrip(User user, Trip trip);
        Task<Trip> AddTripAsync(User user, Trip trip);

        void RemoveTrip(Trip trip);
        Task RemoveTripAsync(Trip trip);

        IReadOnlyList<Trip> GetTripsOfUser(User user);
    }
}
