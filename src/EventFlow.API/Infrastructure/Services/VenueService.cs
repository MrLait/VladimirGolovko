using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services
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
        public VenueService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; private set; }

        /// <inheritdoc/>
        public async Task CreateAsync(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            var allVenues = DbContext.Venues.GetAllAsQueryable().ToList();
            var isVenueContain = allVenues.Any(x => x.Description.Contains(dto.Description));

            if (isVenueContain)
            {
                throw new ValidationException(ExceptionMessages.VenueExist, dto.Description);
            }

            var venue = new Venue { Description = dto.Description, Address = dto.Address, Phone = dto.Phone };
            await DbContext.Venues.CreateAsync(venue);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(VenueDto dto)
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

            await DbContext.Venues.DeleteAsync(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }

        /// <inheritdoc/>
        public IEnumerable<VenueDto> GetAll()
        {
            var venues = DbContext.Venues.GetAllAsQueryable();
            var venuesDto = new List<VenueDto>();
            foreach (var item in venues)
            {
                venuesDto.Add(new VenueDto { Id = item.Id, Address = item.Address, Description = item.Description, Phone = item.Phone });
            }

            return venuesDto;
        }

        /// <inheritdoc/>
        public async Task<VenueDto> GetByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var venue = await DbContext.Venues.GetByIdAsync(id);
            return new VenueDto { Id = venue.Id, Address = venue.Address, Description = venue.Description, Phone = venue.Phone };
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(VenueDto dto)
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

            await DbContext.Venues.UpdateAsync(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }
    }
}
