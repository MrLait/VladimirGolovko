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
                throw new ArgumentException($"Can not create null object: {(SeatDto) null}!");
            }

            bool isSeatContain = ChackThatSeatAlreadyCointainsForThisArea(dto);

            if (isSeatContain)
            {
                throw new ValidationException($"The Seat for this Area with this parameters: Row = {dto.Row} and Number = {dto.Number} - already exists.");
            }

            Seat seat = new Seat { AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row };
            DbContext.Seats.Create(seat);
        }

        /// <inheritdoc/>
        public void Delete(SeatDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not delete null object: {(SeatDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not delete object with id: {dto.Id}!");
            }

            DbContext.Seats.Delete(new Seat { Id = dto.Id });
        }

        /// <inheritdoc/>
        public void Update(SeatDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {(SeatDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            bool isSeatContain = ChackThatSeatAlreadyCointainsForThisArea(dto);

            if (isSeatContain)
            {
                throw new ValidationException($"The Seat for this Area with this parameters: Row = {dto.Row} and Number = {dto.Number} - already exists.");
            }

            DbContext.Seats.Update(new Seat { Id = dto.Id, AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row });
        }

        private bool ChackThatSeatAlreadyCointainsForThisArea(SeatDto dto)
        {
            var allSeatsByAreaId = DbContext.Seats.GetAll().Where(x => x.AreaId == dto.AreaId).ToList();
            var isSeatContain = allSeatsByAreaId.Select(x => x.Number == dto.Row && x.Row == dto.Row).Where(z => z.Equals(true)).ElementAtOrDefault(0);
            return isSeatContain;
        }
    }
}
