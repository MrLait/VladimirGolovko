using System;
using System.Collections.Generic;
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
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            var allVenues = DbContext.Venues.GetAll().ToList();
            var isVenueContain = allVenues.Any(x => x.Description.Contains(dto.Description));

            if (isVenueContain)
            {
                throw new ValidationException(ExceptionMessages.VenueExist, dto.Description);
            }

            Venue venue = new Venue { Description = dto.Description, Address = dto.Address, Phone = dto.Phone };
            DbContext.Venues.Create(venue);
        }

        /// <inheritdoc/>
        public void Delete(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            if (dto.Id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            if (dto.Id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            DbContext.Venues.Delete(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }

        /// <inheritdoc/>
        public IEnumerable<VenueDto> GetAll()
        {
            var venues = DbContext.Venues.GetAll();
            List<VenueDto> venuesDto = new List<VenueDto>();
            foreach (var item in venues)
            {
                venuesDto.Add(new VenueDto { Id = item.Id, Address = item.Address, Description = item.Description, Phone = item.Phone });
            }

            return venuesDto;
        }

        /// <inheritdoc/>
        public VenueDto GetByID(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var venue = DbContext.Venues.GetByID(id);
            return new VenueDto { Id = venue.Id, Address = venue.Address, Description = venue.Description, Phone = venue.Phone };
        }

        /// <inheritdoc/>
        public void Update(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            if (dto.Id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            if (dto.Id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            DbContext.Venues.Update(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }
    }
}
