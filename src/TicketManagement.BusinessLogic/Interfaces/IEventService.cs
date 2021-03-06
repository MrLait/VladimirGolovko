using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IEventService : IService
    {
        void Create(EventDto dto);

        void Delete(EventDto dto);

        void Update(EventDto dto);
    }
}