using System;
using System.Linq;
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
        public void Create(SeatDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            bool isSeatContain = CheckThatSeatAlreadyCointainsForThisArea(dto);

            if (isSeatContain)
            {
                throw new ValidationException(ExceptionMessages.SeatForTheAreaExis, dto.Row, dto.Number);
            }

            Seat seat = new Seat { AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row };
            DbContext.Seats.Create(seat);
        }

        /// <inheritdoc/>
        public void Delete(SeatDto dto)
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

            DbContext.Seats.Delete(new Seat { Id = dto.Id });
        }

        /// <inheritdoc/>
        public void Update(SeatDto dto)
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
                throw new ValidationException(ExceptionMessages.SeatForTheAreaExis, dto.Row, dto.Number);
            }

            DbContext.Seats.Update(new Seat { Id = dto.Id, AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row });
        }

        private bool CheckThatSeatAlreadyCointainsForThisArea(SeatDto dto)
        {
            var allSeatsByAreaId = DbContext.Seats.GetAll().Where(x => x.AreaId == dto.AreaId).ToList();
            var isSeatContain = allSeatsByAreaId.Any(x => x.Number == dto.Row && x.Row == dto.Row);
            return isSeatContain;
        }
    }
}
