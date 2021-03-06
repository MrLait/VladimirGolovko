using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class SeatService : ISeatService
    {
        public SeatService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(SeatDto dto)
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

        public void Delete(SeatDto dto)
        {
            DbContext.Seats.Delete(new Seat { Id = dto.Id });
        }

        public void Update(SeatDto dto)
        {
            DbContext.Seats.Update(new Seat { Id = dto.Id, AreaId = dto.AreaId, Number = dto.Number, Row = dto.Row });
        }
    }
}
