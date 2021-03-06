using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IEventAreaService
    {
        void UpdatePrice(EventAreaDto dto);
    }
}
