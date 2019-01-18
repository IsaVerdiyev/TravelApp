using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;

namespace TravelAppCore.Services
{
    public class TripService : ITripService
    {
        IRepository<City> cityRepository;

        IRepository<Trip> tripRepository;

        public TripService(IRepository<City> cityRepository, IRepository<Trip> tripRepository)
        {
            this.cityRepository = cityRepository;
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

        public void RemoveTrip(DeleteByIdSpecification<Trip> specification)
        {
            tripRepository.DeleteBySpec(specification);
        }

        public async Task RemoveTripAsync(DeleteByIdSpecification<Trip> specification)
        {
            await tripRepository.DeleteBySpecAsync(specification);
        }

        public void ChangeArivalDate(Trip trip, DateTime arrivalDate)
        {
            trip.ArriavalDate = arrivalDate;
            tripRepository.Update(trip);
        }

        public async Task ChangeArrivalDateAsync(Trip trip, DateTime arrivalDate)
        {
            trip.ArriavalDate = arrivalDate;
            await tripRepository.UpdateAsync(trip);
        }

        public void ChangeDepartureDate(Trip trip, DateTime departureDate)
        {
            trip.DepartureDate = departureDate;
            tripRepository.Update(trip);
        }

        public async Task ChangeDepartureDateAsync(Trip trip, DateTime departureDate)
        {
            trip.DepartureDate = departureDate;
            await tripRepository.UpdateAsync(trip);
        }

        public IReadOnlyList<Trip> GetTripsOfUser(User user)
        {
            return tripRepository.List(new UserTripsSpecification(user.Id));
        }

        public async Task<IReadOnlyList<Trip>> GetTripsOfUserAsync(User user)
        {
            return await tripRepository.ListAsync(new UserTripsSpecification(user.Id));
        }
    }
}
