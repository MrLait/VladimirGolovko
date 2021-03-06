using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class EventAreaService : IEventAreaService
    {
        public EventAreaService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void UpdatePrice(EventAreaDto dto)
        {
            var currentEventAreas = DbContext.EventAreas.GetByID(dto.Id);
            currentEventAreas.Price = dto.Price;
            DbContext.EventAreas.Update(currentEventAreas);
        }
    }
}
