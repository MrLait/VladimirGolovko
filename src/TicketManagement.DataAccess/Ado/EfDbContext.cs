using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.DataAccess.Ado
{
    public class EfDbContext : DbContext, IDbContext
    {
        private EfRepository<Area> _areaRepository;
        private EfRepository<EventArea> _eventAreaRepository;
        private EfRepositoryUsingStoredProcedure<Event> _eventRepository;
        private EfRepository<EventSeat> _eventSeatRepository;
        private EfRepository<Layout> _layoutRepository;
        private EfRepository<Seat> _seatRepository;
        private EfRepository<Venue> _venueRepository;
        private EfRepository<Basket> _basketRepository;
        private EfRepository<PurchaseHistory> _purchaseHistoryRepository;

        public EfDbContext(DbContextOptions<EfDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public EfDbContext(string connectionString)
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
        public IRepository<Area> Areas => _areaRepository ??= new EfRepository<Area>(this);

        /// <summary>
        /// Gets basket table property.
        /// </summary>
        public IRepository<Basket> Baskets => _basketRepository ??= new EfRepository<Basket>(this);

        /// <summary>
        /// Gets purchase history table property.
        /// </summary>
        public IRepository<PurchaseHistory> PurchaseHistories => _purchaseHistoryRepository ??= new EfRepository<PurchaseHistory>(this);

        /// <summary>
        /// Gets event table property.
        /// </summary>
        public IRepository<Event> Events => _eventRepository ??= new EfRepositoryUsingStoredProcedure<Event>(this);

        /// <summary>
        /// Gets eventArea table property.
        /// </summary>
        public IRepository<EventArea> EventAreas => _eventAreaRepository ??= new EfRepository<EventArea>(this);

        /// <summary>
        /// Gets eventSeat table property.
        /// </summary>
        public IRepository<EventSeat> EventSeats => _eventSeatRepository ??= new EfRepository<EventSeat>(this);

        /// <summary>
        /// Gets layout table property.
        /// </summary>
        public IRepository<Layout> Layouts => _layoutRepository ??= new EfRepository<Layout>(this);

        /// <summary>
        /// Gets seat table property.
        /// </summary>
        public IRepository<Seat> Seats => _seatRepository ??= new EfRepository<Seat>(this);

        /// <summary>
        /// Gets venue table property.
        /// </summary>
        public IRepository<Venue> Venues => _venueRepository ??= new EfRepository<Venue>(this);

        public virtual DbSet<Area> Area { get; set; }

        public virtual DbSet<Event> Event { get; set; }

        public virtual DbSet<EventArea> EventArea { get; set; }

        public virtual DbSet<EventSeat> EventSeat { get; set; }

        public virtual DbSet<Layout> Layout { get; set; }

        public virtual DbSet<Seat> Seat { get; set; }

        public virtual DbSet<Venue> Venue { get; set; }

        public virtual DbSet<Basket> Basket { get; set; }

        public virtual DbSet<PurchaseHistory> PurchaseHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
