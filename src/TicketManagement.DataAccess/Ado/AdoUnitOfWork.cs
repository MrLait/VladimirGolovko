using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.ModelsRepository;

namespace TicketManagement.DataAccess.Ado
{
    public class AdoUnitOfWork : IUnitOfWork
    {
        private readonly string _connectionString;

        private AreaRepository _areaRepository;
        private EventAreaRepository _eventAreaRepository;
        private EventRepository _eventRepository;
        private EventSeatRepository _eventSeatRepository;
        private LayoutRepository _layoutRepository;
        private SeatRepository _seatRepository;
        private VenueRepository _venueRepository;

        public AdoUnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IRepository<Area> Areas => _areaRepository ??= new AreaRepository(_connectionString);

        public IRepository<Event> Events => _eventRepository ??= new EventRepository(_connectionString);

        public IRepository<EventArea> EventAreas => _eventAreaRepository ??= new EventAreaRepository(_connectionString);

        public IRepository<EventSeat> EventSeats => _eventSeatRepository ??= new EventSeatRepository(_connectionString);

        public IRepository<Layout> Layouts => _layoutRepository ??= new LayoutRepository(_connectionString);

        public IRepository<Seat> Seats => _seatRepository ??= new SeatRepository(_connectionString);

        public IRepository<Venue> Venues => _venueRepository ??= new VenueRepository(_connectionString);
    }
}
