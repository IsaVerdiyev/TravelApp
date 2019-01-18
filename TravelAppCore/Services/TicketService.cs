using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Specifications;

namespace TravelAppCore.Services
{
    public class TicketService : ITicketService
    {

        IRepository<Ticket> ticketRepository;

        public TicketService(IRepository<Ticket> ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        public Ticket AddTicket(Trip trip, Ticket ticket)
        {
            ticket.TripId = trip.Id;
            return ticketRepository.Add(ticket);
        }

        public async  Task<Ticket> AddTicketAsync(Trip trip, Ticket ticket)
        {
            ticket.TripId = trip.Id;
            return await ticketRepository.AddAsync(ticket);
        }

        public IReadOnlyList<Ticket> GetTicketsOfTrip(Trip trip)
        {
            return ticketRepository.List(new TripTicketsSpecification(trip));
        }

        public async Task<IReadOnlyList<Ticket>> GetTicketsOfTripAsync(Trip trip)
        {
            return await ticketRepository.ListAsync(new TripTicketsSpecification(trip));
        }

        public void RemoveTicket(DeleteByIdSpecification<Ticket> specification)
        {
            ticketRepository.DeleteBySpec(specification);
        }

        public async Task RemoveTicketAsync(DeleteByIdSpecification<Ticket> specification)
        {
            await ticketRepository.DeleteBySpecAsync(specification);
        }
    }
}
