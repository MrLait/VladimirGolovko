using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IEventSeatService : IService
    {
        void UpdateState(EventSeatDto dto);
    }
}
