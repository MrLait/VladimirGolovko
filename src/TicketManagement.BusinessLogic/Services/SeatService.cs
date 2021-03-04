using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class SeatService : AbstractService<SeatDto>
    {
        public SeatService(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public override void Create(SeatDto dto)
        {
            var allSeatsByAreaId = DbContext.Seats.GetAll().Where(x => x.AreaId == dto.AreaId).ToList();
            var isSeatContain = allSeatsByAreaId.Select(x => x.Number == dto.Row && x.Row == dto.Row).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isSeatContain)
            {
                throw new ValidationException($"The Seat for this Area with this parameters: Row = {dto.Row} and Number = {dto.Number} - already exists.");
            }
            else
            {
                Seat seat = new Seat { AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row };
                DbContext.Seats.Create(seat);
            }
        }
    }
}
