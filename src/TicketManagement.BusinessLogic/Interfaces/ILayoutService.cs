using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface ILayoutService : IService
    {
        void Create(LayoutDto dto);

        void Delete(LayoutDto dto);

        void Update(LayoutDto dto);
    }
}