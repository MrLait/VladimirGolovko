using System;
using System.Collections.Generic;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
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
        internal EventAreaService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public IEnumerable<EventAreaDto> GetAll()
        {
            var eventAreas = DbContext.EventAreas.GetAll();
            List<EventAreaDto> eventAreasDto = new List<EventAreaDto>();
            foreach (var item in eventAreas)
            {
                eventAreasDto.Add(new EventAreaDto { Id = item.Id, Description = item.Description, CoordX = item.CoordX, CoordY = item.CoordY, EventId = item.EventId, Price = item.Price });
            }

            return eventAreasDto;
        }

        /// <inheritdoc/>
        public EventAreaDto GetByID(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var eventArea = DbContext.EventAreas.GetByID(id);
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
        public void UpdatePrice(EventAreaDto dto)
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

            var currentEventAreas = DbContext.EventAreas.GetByID(dto.Id);
            currentEventAreas.Price = dto.Price;
            DbContext.EventAreas.Update(currentEventAreas);
        }
    }
}
