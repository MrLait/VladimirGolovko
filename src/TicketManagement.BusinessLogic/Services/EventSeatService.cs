using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class EventSeatService : IEventSeatService
    {
        public EventSeatService(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; private set; }

        public void UpdateState(EventSeatDto dto)
        {
            var currentEventSeat = DbContext.EventSeats.GetByID(dto.Id);
            currentEventSeat.State = dto.State;
            DbContext.EventSeats.Update(currentEventSeat);
        }
    }
}