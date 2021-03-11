using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.DataAccess.Ado
{
    /// <summary>
    /// Database context with tables.
    /// </summary>
    internal class AdoDbContext : IDbContext
    {
        private AdoUsingParametersRepository<Area> _areaRepository;
        private AdoUsingParametersRepository<EventArea> _eventAreaRepository;
        private AdoUsingStoredProcedureRepository<Event> _eventRepository;
        private AdoUsingParametersRepository<EventSeat> _eventSeatRepository;
        private AdoUsingParametersRepository<Layout> _layoutRepository;
        private AdoUsingParametersRepository<Seat> _seatRepository;
        private AdoUsingParametersRepository<Venue> _venueRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdoDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public AdoDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets property connection string to database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Gets area table property.
        /// </summary>
        public IRepository<Area> Areas => _areaRepository ??= new AdoUsingParametersRepository<Area>(ConnectionString);

        /// <summary>
        /// Gets event table property.
        /// </summary>
        public IRepository<Event> Events => _eventRepository ??= new AdoUsingStoredProcedureRepository<Event>(ConnectionString);

        /// <summary>
        /// Gets eventArea table property.
        /// </summary>
        public IRepository<EventArea> EventAreas => _eventAreaRepository ??= new AdoUsingParametersRepository<EventArea>(ConnectionString);

        /// <summary>
        /// Gets eventSeat table property.
        /// </summary>
        public IRepository<EventSeat> EventSeats => _eventSeatRepository ??= new AdoUsingParametersRepository<EventSeat>(ConnectionString);

        /// <summary>
        /// Gets layout table property.
        /// </summary>
        public IRepository<Layout> Layouts => _layoutRepository ??= new AdoUsingParametersRepository<Layout>(ConnectionString);

        /// <summary>
        /// Gets seat table property.
        /// </summary>
        public IRepository<Seat> Seats => _seatRepository ??= new AdoUsingParametersRepository<Seat>(ConnectionString);

        /// <summary>
        /// Gets venue table property.
        /// </summary>
        public IRepository<Venue> Venues => _venueRepository ??= new AdoUsingParametersRepository<Venue>(ConnectionString);
    }
}
