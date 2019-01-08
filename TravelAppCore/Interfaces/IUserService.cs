using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface IUserService
    {
        Trip AddTrip(User user, Trip trip);
        Task<Trip> AddTripAsync(User user, Trip trip);

        void RemoveTrip(User user, Trip trip);
        Task RemoveTripAsync(User user, Trip trip);
    }
}
