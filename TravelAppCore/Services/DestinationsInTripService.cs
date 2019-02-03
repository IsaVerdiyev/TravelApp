using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;

namespace TravelAppCore.Services
{
    public class DestinationsInTripService : IDestinationsInTripService
    {
        IRepository<DestinationCityInTrip> destinationsRepository;

        public DestinationsInTripService(IRepository<DestinationCityInTrip> destinationsInTripService)
        {
            this.destinationsRepository = destinationsInTripService;
        }

        public DestinationCityInTrip AddDestinationInTrip(Trip trip, DestinationCityInTrip destinationInTrip)
        {
            destinationInTrip.TripId = trip.Id;
            return destinationsRepository.Add(destinationInTrip);
           
        }

        public async Task<DestinationCityInTrip> AddDestinationInTripAsync(Trip trip, DestinationCityInTrip destinationInTrip)
        {
            destinationInTrip.TripId = trip.Id;
            return await destinationsRepository.AddAsync(destinationInTrip);
        }

        public IReadOnlyList<DestinationCityInTrip> GetDestinationsOfTrip(Trip trip)
        {
            return destinationsRepository.List(new TripCitiesSpecification(trip));
        }

        public async Task<IReadOnlyList<DestinationCityInTrip>> GetDestinationsOfTripAsync(Trip trip)
        {
            return await destinationsRepository.ListAsync(new TripCitiesSpecification(trip));
        }

        public void RemoveDestinationFromTrip(DeleteByIdSpecification<DestinationCityInTrip> specification)
        {
            destinationsRepository.DeleteBySpec(specification);
        }

        public async Task RemoveDestinationFromTripAsync(DeleteByIdSpecification<DestinationCityInTrip> specification)
        {
            await destinationsRepository.DeleteBySpecAsync(specification);
        }
    }
}
