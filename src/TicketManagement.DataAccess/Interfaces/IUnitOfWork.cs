using System;
using TicketManagement.DataAccess.Domain.Models;

namespace TicketManagement.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Area> Areas { get; }

        IRepository<Event> Events { get; }

        IRepository<EventArea> EventAreas { get; }

        IRepository<EventSeat> EventSeats { get; }

        IRepository<Layout> Layouts { get; }

        IRepository<Seat> Seats { get; }

        IRepository<Venue> Venues { get; }
    }
}
