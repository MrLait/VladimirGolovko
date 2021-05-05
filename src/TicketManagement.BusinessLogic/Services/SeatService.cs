using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
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
        internal SeatService(IDbContext dbContext) => DbContext = dbContext;

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

            bool isSeatContain = await CheckThatSeatAlreadyCointainsForThisAreaAsync(dto);

            if (isSeatContain)
            {
                throw new ValidationException(ExceptionMessages.SeatForTheAreaExis, dto.Row, dto.Number);
            }

            Seat seat = new Seat { AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row };
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
        public async Task<IEnumerable<SeatDto>> GetAllAsync()
        {
            var seats = await DbContext.Seats.GetAllAsync();
            List<SeatDto> seatDto = new List<SeatDto>();
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
        public async Task<SeatDto> GetByIDAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var seat = await DbContext.Seats.GetByIDAsync(id);
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

            bool isSeatContain = await CheckThatSeatAlreadyCointainsForThisAreaAsync(dto);

            if (isSeatContain)
            {
                throw new ValidationException(ExceptionMessages.SeatForTheAreaExis, dto.Row, dto.Number);
            }

            await DbContext.Seats.UpdateAsync(new Seat { Id = dto.Id, AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row });
        }

        private async Task<bool> CheckThatSeatAlreadyCointainsForThisAreaAsync(SeatDto dto)
        {
            var allSeatsByAreaId = (await DbContext.Seats.GetAllAsync()).Where(x => x.AreaId == dto.AreaId).ToList();
            var isSeatContain = allSeatsByAreaId.Any(x => x.Number == dto.Row && x.Row == dto.Row);
            return isSeatContain;
        }
    }
}
