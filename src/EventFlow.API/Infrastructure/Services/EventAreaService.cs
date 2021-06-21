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
    /// Event area service class.
    /// </summary>
    internal class EventAreaService : IEventAreaService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public EventAreaService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public IEnumerable<EventAreaDto> GetAll()
        {
            var eventAreas = DbContext.EventAreas.GetAllAsQueryable();
            var eventAreasDto = new List<EventAreaDto>();
            foreach (var item in eventAreas)
            {
                eventAreasDto.Add(new EventAreaDto { Id = item.Id, Description = item.Description, CoordX = item.CoordX, CoordY = item.CoordY, EventId = item.EventId, Price = item.Price });
            }

            return eventAreasDto;
        }

        /// <inheritdoc/>
        public IEnumerable<EventAreaDto> GetByEventId(int id)
        {
            var eventAreas = DbContext.EventAreas.GetAllAsQueryable().Where(x => x.EventId == id);

            var eventAreasDto = new List<EventAreaDto>();
            foreach (var item in eventAreas)
            {
                eventAreasDto.Add(new EventAreaDto { Id = item.Id, Description = item.Description, CoordX = item.CoordX, CoordY = item.CoordY, EventId = item.EventId, Price = item.Price });
            }

            return eventAreasDto;
        }

        /// <inheritdoc/>
        public async Task<EventAreaDto> GetByIDAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var eventArea = await DbContext.EventAreas.GetByIDAsync(id);
            var eventAreaDto = new EventAreaDto
            {
                Id = eventArea.Id,
                Description = eventArea.Description,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                EventId = eventArea.EventId,
                Price = eventArea.Price,
            };

            return eventAreaDto;
        }

        /// <inheritdoc/>
        public async Task UpdatePriceAsync(EventAreaDto dto)
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

            if (dto.Price < 0)
            {
                throw new ValidationException(ExceptionMessages.PriceIsNegative);
            }

            if (dto.Price == 0)
            {
                throw new ValidationException(ExceptionMessages.PriceIsZero);
            }

            var currentEventAreas = await DbContext.EventAreas.GetByIDAsync(dto.Id);
            currentEventAreas.Price = dto.Price;
            await DbContext.EventAreas.UpdateAsync(currentEventAreas);
        }

        /// <inheritdoc/>
        public async Task UpdatePriceAsync(IEnumerable<EventAreaDto> dto)
        {
            foreach (var item in dto)
            {
                await UpdatePriceAsync(item);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<EventAreaDto> GetAllEventAreasForEvent(EventDto dto)
        {
            var allEventAreasForEvent = DbContext.EventAreas.GetAllAsQueryable().Where(x => x.EventId == dto.Id);
            var eventAreasDto = new List<EventAreaDto>();

            foreach (var item in allEventAreasForEvent)
            {
                eventAreasDto.Add(new EventAreaDto
                {
                    Id = item.Id,
                    CoordX = item.CoordX,
                    CoordY = item.CoordY,
                    Description = item.Description,
                    EventId = item.EventId,
                    Price = item.Price,
                });
            }

            return eventAreasDto;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(EventAreaDto dto)
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

            await DbContext.EventAreas.DeleteAsync(new EventArea { Id = dto.Id });
        }
    }
}
