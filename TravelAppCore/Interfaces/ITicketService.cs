using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Specifications;

namespace TravelAppCore.Interfaces
{
    public interface ITicketService
    {
        Ticket AddTicket(Trip trip, Ticket ticket);
        Task<Ticket> AddTicketAsync(Trip trip, Ticket ticket);

        void RemoveTicket(DeleteByIdSpecification<Ticket> specification);
        Task RemoveTicketAsync(DeleteByIdSpecification<Ticket> specification);
    }
}
