using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IEventSeatService
    {
        void UpdateState(EventSeatDto dto);
    }
}
