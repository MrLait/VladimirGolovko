using System;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Venue service class.
    /// </summary>
    internal class VenueService : IVenueService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        internal VenueService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; private set; }

        /// <inheritdoc/>
        public void Create(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not create null object: {(VenueDto) null}!");
            }

            var allVenues = DbContext.Venues.GetAll().ToList();
            var isVenueContain = allVenues.Select(x => x.Description.Contains(dto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isVenueContain)
            {
                throw new ValidationException($"The Venue with this description: {dto.Description} - already exists.");
            }

            Venue venue = new Venue { Description = dto.Description, Address = dto.Address, Phone = dto.Phone };
            DbContext.Venues.Create(venue);
        }

        /// <inheritdoc/>
        public void Delete(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not delete null object: {(VenueDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not delete object with id: {dto.Id}!");
            }

            DbContext.Venues.Delete(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }

        /// <inheritdoc/>
        public void Update(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {(VenueDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            DbContext.Venues.Update(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }
    }
}
