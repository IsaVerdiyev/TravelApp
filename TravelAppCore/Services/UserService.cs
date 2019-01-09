using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

namespace TravelAppCore.Services
{
    class UserService : IUserService
    {

        IRepository<Trip> tripRepository;

        public UserService(IRepository<Trip> tripRepository)
        {
            this.tripRepository = tripRepository;
        }

        public Trip AddTrip(User user, Trip trip)
        {
            trip.UserId = user.Id;
            return tripRepository.Add(trip);            
        }

        public async Task<Trip> AddTripAsync(User user, Trip trip)
        {
            trip.UserId = user.Id;
            return await tripRepository.AddAsync(trip);

        }

        public void RemoveTrip(Trip trip)
        {
            tripRepository.Delete(trip);
        }

        public async Task RemoveTripAsync(Trip trip)
        {
            await tripRepository.DeleteAsync(trip);
        }
    }
}
