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
        IRepository<DestinationCityInTrip> cityRepository;

        public DestinationsInTripService(IRepository<DestinationCityInTrip> cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        public DestinationCityInTrip AddDestinationInTrip(Trip trip, DestinationCityInTrip destinationInTrip)
        {
            destinationInTrip.TripId = trip.Id;
            return cityRepository.Add(destinationInTrip);
           
        }

        public async Task<DestinationCityInTrip> AddDestinationInTripAsync(Trip trip, DestinationCityInTrip destinationInTrip)
        {
            destinationInTrip.TripId = trip.Id;
            return await cityRepository.AddAsync(destinationInTrip);
        }

        public IReadOnlyList<DestinationCityInTrip> GetDestinationsOfTrip(Trip trip)
        {
            return cityRepository.List(new TripCitiesSpecification(trip));
        }

        public async Task<IReadOnlyList<DestinationCityInTrip>> GetDestinationsOfTripAsync(Trip trip)
        {
            return await cityRepository.ListAsync(new TripCitiesSpecification(trip));
        }

        public void RemoveDestinationFromTrip(DeleteByIdSpecification<DestinationCityInTrip> specification)
        {
            cityRepository.DeleteBySpec(specification);
        }

        public async Task RemoveDestinationFromTripAsync(DeleteByIdSpecification<DestinationCityInTrip> specification)
        {
            await cityRepository.DeleteBySpecAsync(specification);
        }
    }
}
