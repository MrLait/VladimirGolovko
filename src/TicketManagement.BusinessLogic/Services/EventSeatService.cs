﻿using System.Collections.Generic;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
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
        internal EventSeatService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public IEnumerable<EventSeatDto> GetAll()
        {
            var eventSeats = DbContext.EventSeats.GetAll();
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
        public EventSeatDto GetByID(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var eventSeat = DbContext.EventSeats.GetByID(id);
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
        public void UpdateState(EventSeatDto dto)
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

            var currentEventSeat = DbContext.EventSeats.GetByID(dto.Id);
            currentEventSeat.State = dto.State;
            DbContext.EventSeats.Update(currentEventSeat);
        }
    }
}