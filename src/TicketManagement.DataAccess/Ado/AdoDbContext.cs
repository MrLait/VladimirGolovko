using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.DataAccess.Ado
{
    public class AdoDbContext : IDbContext
    {
        private readonly string _connectionString;

        private AdoUsingParametersRepository<Area> _areaRepository;
        private AdoUsingParametersRepository<EventArea> _eventAreaRepository;
        private AdoUsingStoredProcedureRepository<Event> _eventRepository;
        private AdoUsingParametersRepository<EventSeat> _eventSeatRepository;
        private AdoUsingParametersRepository<Layout> _layoutRepository;
        private AdoUsingParametersRepository<Seat> _seatRepository;
        private AdoUsingParametersRepository<Venue> _venueRepository;

        public AdoDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IRepository<Area> Areas => _areaRepository ??= new AdoUsingParametersRepository<Area>(_connectionString);

        public IRepository<Event> Events => _eventRepository ??= new AdoUsingStoredProcedureRepository<Event>(_connectionString);

        public IRepository<EventArea> EventAreas => _eventAreaRepository ??= new AdoUsingParametersRepository<EventArea>(_connectionString);

        public IRepository<EventSeat> EventSeats => _eventSeatRepository ??= new AdoUsingParametersRepository<EventSeat>(_connectionString);

        public IRepository<Layout> Layouts => _layoutRepository ??= new AdoUsingParametersRepository<Layout>(_connectionString);

        public IRepository<Seat> Seats => _seatRepository ??= new AdoUsingParametersRepository<Seat>(_connectionString);

        public IRepository<Venue> Venues => _venueRepository ??= new AdoUsingParametersRepository<Venue>(_connectionString);
    }
}
