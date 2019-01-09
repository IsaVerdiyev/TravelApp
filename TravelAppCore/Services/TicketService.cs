using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;

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

        public void RemoveTicket(Ticket ticket)
        {
            ticketRepository.Delete(ticket);
        }

        public async Task RemoveTicketAsync(Ticket ticket)
        {
            await ticketRepository.DeleteAsync(ticket);
        }
    }
}
