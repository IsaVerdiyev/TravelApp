﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppCore.Interfaces
{
    public interface ITicketService
    {
        Ticket AddTicket(Trip trip, Ticket ticket);
        Task<Ticket> AddTicketAsync(Trip trip, Ticket ticket);

        void RemoveTicket(Ticket ticket);
        Task RemoveTicketAsync(Ticket ticket);
    }
}
