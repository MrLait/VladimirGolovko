using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface ISeatService : IService
    {
        void Create(SeatDto dto);

        void Delete(SeatDto dto);

        void Update(SeatDto dto);
    }
}