using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IEventAreaService : IService
    {
        void UpdatePrice(EventAreaDto dto);
    }
}
