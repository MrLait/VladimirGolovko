using System;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    public class EventSeatService : IEventSeatService
    {
        public EventSeatService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void UpdateState(EventSeatDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {dto}!");
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