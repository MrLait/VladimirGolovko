using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IAreaService : IService
    {
        void Create(AreaDto dto);

        void Delete(AreaDto dto);

        void Update(AreaDto dto);
    }
}