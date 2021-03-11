﻿using System;
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
        public void UpdatePrice(EventAreaDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {(EventAreaDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            if (dto.Price < 0)
            {
                throw new ArgumentException($"Can not update object with price less then zero: {dto.Price}!");
            }

            var currentEventAreas = DbContext.EventAreas.GetByID(dto.Id);
            currentEventAreas.Price = dto.Price;
            DbContext.EventAreas.Update(currentEventAreas);
        }
    }
}