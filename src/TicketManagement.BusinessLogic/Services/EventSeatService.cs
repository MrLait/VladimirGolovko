﻿using System;
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
        public void UpdateState(EventSeatDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {(EventSeatDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            if (dto.State < 0)
            {
                throw new ArgumentException($"Can not update object with state less then zero: {dto.State}!");
            }

            var currentEventSeat = DbContext.EventSeats.GetByID(dto.Id);
            currentEventSeat.State = dto.State;
            DbContext.EventSeats.Update(currentEventSeat);
        }
    }
}