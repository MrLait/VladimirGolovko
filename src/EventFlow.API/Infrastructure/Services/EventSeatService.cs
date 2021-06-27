using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Enums;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services
{
    /// <summary>
    /// Event seat service class.
    /// </summary>
    internal class EventSeatService : IEventSeatService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public EventSeatService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public IEnumerable<EventSeatDto> GetAll()
        {
            var eventSeats = DbContext.EventSeats.GetAllAsQueryable().ToList();

            return eventSeats.Select(eventSeat => new EventSeatDto
                {
                    Id = eventSeat.Id,
                    EventAreaId = eventSeat.EventAreaId,
                    Number = eventSeat.Number,
                    Row = eventSeat.Row,
                    State = (States) eventSeat.State,
                })
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<EventSeatDto> GetByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var eventSeat = await DbContext.EventSeats.GetByIdAsync(id);
            var eventSeatDto = new EventSeatDto
            {
                Id = eventSeat.Id,
                EventAreaId = eventSeat.EventAreaId,
                Number = eventSeat.Number,
                Row = eventSeat.Row,
                State = (States)eventSeat.State,
            };
            return eventSeatDto;
        }

        /// <inheritdoc/>
        public IEnumerable<EventSeatDto> GetByEventAreaId(EventAreaDto dto)
        {
            var eventSeats = DbContext.EventSeats.GetAllAsQueryable().OrderBy(x => x.EventAreaId).Where(x => x.EventAreaId == dto.Id);

            var eventSeatsDto = new List<EventSeatDto>();
            foreach (var item in eventSeats)
            {
                eventSeatsDto.Add(new EventSeatDto
                {
                    Id = item.Id,
                    EventAreaId = item.EventAreaId,
                    Number = item.Number,
                    Row = item.Row,
                    State = (States)item.State,
                });
            }

            return eventSeatsDto;
        }

        /// <inheritdoc/>
        public async Task UpdateStateAsync(EventSeatDto dto)
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

            var currentEventSeat = await DbContext.EventSeats.GetByIdAsync(dto.Id);
            currentEventSeat.State = (int)dto.State;
            await DbContext.EventSeats.UpdateAsync(currentEventSeat);
        }
    }
}