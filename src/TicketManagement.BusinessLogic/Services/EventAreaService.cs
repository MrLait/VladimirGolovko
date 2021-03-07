using System;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    public class EventAreaService : IEventAreaService
    {
        public EventAreaService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void UpdatePrice(EventAreaDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {dto}!");
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
