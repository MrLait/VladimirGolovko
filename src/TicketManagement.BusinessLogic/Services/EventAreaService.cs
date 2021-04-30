using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Event area service class.
    /// </summary>
    public class EventAreaService : IEventAreaService
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
        public async Task<IEnumerable<EventAreaDto>> GetAllAsync()
        {
            var eventAreas = await DbContext.EventAreas.GetAllAsync();
            List<EventAreaDto> eventAreasDto = new List<EventAreaDto>();
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
                throw new ValidationException(ExceptionMessages.PriceIsNegative, dto.Price);
            }

            var currentEventAreas = await DbContext.EventAreas.GetByIDAsync(dto.Id);
            currentEventAreas.Price = dto.Price;
            await DbContext.EventAreas.UpdateAsync(currentEventAreas);
        }
    }
}
