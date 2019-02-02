using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Specifications;

namespace TravelAppCore.Interfaces
{
    public interface IDestinationsInTripService
    {
        DestinationCityInTrip AddDestinationInTrip(Trip trip, DestinationCityInTrip destinationInTrip);
        Task<DestinationCityInTrip> AddDestinationInTripAsync(Trip trip, DestinationCityInTrip destinationInTrip);

        void RemoveDestinationFromTrip(DeleteByIdSpecification<DestinationCityInTrip> deleteByIdSpecification);
        Task RemoveDestinationFromTripAsync(DeleteByIdSpecification<DestinationCityInTrip> deleteByIdSpecification);

        IReadOnlyList<DestinationCityInTrip> GetDestinationsOfTrip(Trip trip);
        Task<IReadOnlyList<DestinationCityInTrip>> GetDestinationsOfTripAsync(Trip trip);

    }
}
