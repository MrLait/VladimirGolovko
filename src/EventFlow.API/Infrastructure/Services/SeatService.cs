﻿using System.Collections.Generic;
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
    /// Seat service class.
    /// </summary>
    internal class SeatService : ISeatService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public SeatService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task CreateAsync(SeatDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            bool isSeatContain = CheckThatSeatAlreadyCointainsForThisArea(dto);

            if (isSeatContain)
            {
                throw new ValidationException(ExceptionMessages.SeatForTheAreaExist, dto.Row, dto.Number);
            }

            var seat = new Seat { AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row };
            await DbContext.Seats.CreateAsync(seat);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(SeatDto dto)
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

            await DbContext.Seats.DeleteAsync(new Seat { Id = dto.Id });
        }

        /// <inheritdoc/>
        public IEnumerable<SeatDto> GetAll()
        {
            var seats = DbContext.Seats.GetAllAsQueryable();
            var seatDto = new List<SeatDto>();
            foreach (var seat in seats)
            {
                seatDto.Add(new SeatDto
                {
                    Id = seat.Id,
                    AreaId = seat.AreaId,
                    Number = seat.Number,
                    Row = seat.Row,
                });
            }

            return seatDto;
        }

        /// <inheritdoc/>
        public async Task<SeatDto> GetByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var seat = await DbContext.Seats.GetByIdAsync(id);
            var seatDto = new SeatDto
            {
                Id = seat.Id,
                AreaId = seat.AreaId,
                Number = seat.Number,
                Row = seat.Row,
            };

            return seatDto;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(SeatDto dto)
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

            bool isSeatContain = CheckThatSeatAlreadyCointainsForThisArea(dto);

            if (isSeatContain)
            {
                throw new ValidationException(ExceptionMessages.SeatForTheAreaExist, dto.Row, dto.Number);
            }

            await DbContext.Seats.UpdateAsync(new Seat { Id = dto.Id, AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row });
        }

        private bool CheckThatSeatAlreadyCointainsForThisArea(SeatDto dto)
        {
            var allSeatsByAreaId = DbContext.Seats.GetAllAsQueryable().Where(x => x.AreaId == dto.AreaId).ToList();
            var isSeatContain = allSeatsByAreaId.Any(x => x.Number == dto.Row && x.Row == dto.Row);
            return isSeatContain;
        }
    }
}
