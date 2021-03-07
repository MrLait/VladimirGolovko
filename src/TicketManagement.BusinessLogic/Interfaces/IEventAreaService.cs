using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    public interface IEventAreaService : IService
    {
        void UpdatePrice(EventAreaDto dto);
    }
}
