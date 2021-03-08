namespace TicketManagement.DataAccess.Interfaces
{
    using TicketManagement.DataAccess.Domain.Models;

    /// <summary>
    /// Databese context interface.
    /// </summary>
    internal interface IDbContext
    {
        /// <summary>
        /// Gets database connection string.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets area table repository.
        /// </summary>
        IRepository<Area> Areas { get; }

        /// <summary>
        /// Gets event table repository.
        /// </summary>
        IRepository<Event> Events { get; }

        /// <summary>
        /// Gets eventArea table repository.
        /// </summary>
        IRepository<EventArea> EventAreas { get; }

        /// <summary>
        /// Gets eventSeat table repository.
        /// </summary>
        IRepository<EventSeat> EventSeats { get; }

        /// <summary>
        /// Gets layout table repository.
        /// </summary>
        IRepository<Layout> Layouts { get; }

        /// <summary>
        /// Gets seat table repository.
        /// </summary>
        IRepository<Seat> Seats { get; }

        /// <summary>
        /// Gets venue table repository.
        /// </summary>
        IRepository<Venue> Venues { get; }
    }
}
