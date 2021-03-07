using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.DataAccess.Ado
{
    internal class AdoDbContext : IDbContext
    {
        private AdoUsingParametersRepository<Area> _areaRepository;
        private AdoUsingParametersRepository<EventArea> _eventAreaRepository;
        private AdoUsingStoredProcedureRepository<Event> _eventRepository;
        private AdoUsingParametersRepository<EventSeat> _eventSeatRepository;
        private AdoUsingParametersRepository<Layout> _layoutRepository;
        private AdoUsingParametersRepository<Seat> _seatRepository;
        private AdoUsingParametersRepository<Venue> _venueRepository;

        public AdoDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public IRepository<Area> Areas => _areaRepository ??= new AdoUsingParametersRepository<Area>(ConnectionString);

        public IRepository<Event> Events => _eventRepository ??= new AdoUsingStoredProcedureRepository<Event>(ConnectionString);

        public IRepository<EventArea> EventAreas => _eventAreaRepository ??= new AdoUsingParametersRepository<EventArea>(ConnectionString);

        public IRepository<EventSeat> EventSeats => _eventSeatRepository ??= new AdoUsingParametersRepository<EventSeat>(ConnectionString);

        public IRepository<Layout> Layouts => _layoutRepository ??= new AdoUsingParametersRepository<Layout>(ConnectionString);

        public IRepository<Seat> Seats => _seatRepository ??= new AdoUsingParametersRepository<Seat>(ConnectionString);

        public IRepository<Venue> Venues => _venueRepository ??= new AdoUsingParametersRepository<Venue>(ConnectionString);
    }
}
