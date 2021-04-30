﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Event seat service class.
    /// </summary>
    public class EventSeatService : IEventSeatService
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
        public async Task<IEnumerable<EventSeatDto>> GetAllAsync()
        {
            var eventSeats = await DbContext.EventSeats.GetAllAsync();
            List<EventSeatDto> eventSeatsDto = new List<EventSeatDto>();
            foreach (var eventSeat in eventSeats)
            {
                eventSeatsDto.Add(new EventSeatDto
                {
                    Id = eventSeat.Id, EventAreaId = eventSeat.EventAreaId, Number = eventSeat.Number, Row = eventSeat.Row, State = eventSeat.State,
                });
            }

            return eventSeatsDto;
        }

        /// <inheritdoc/>
        public async Task<EventSeatDto> GetByIDAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var eventSeat = await DbContext.EventSeats.GetByIDAsync(id);
            var eventSeatDto = new EventSeatDto
            {
                Id = eventSeat.Id,
                EventAreaId = eventSeat.EventAreaId,
                Number = eventSeat.Number,
                Row = eventSeat.Row,
                State = eventSeat.State,
            };

            return eventSeatDto;
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

            if (dto.State < 0)
            {
                throw new ValidationException(ExceptionMessages.StateIsNegative, dto.State);
            }

            var currentEventSeat = await DbContext.EventSeats.GetByIDAsync(dto.Id);
            currentEventSeat.State = dto.State;
            await DbContext.EventSeats.UpdateAsync(currentEventSeat);
        }
    }
}