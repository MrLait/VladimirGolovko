using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IVenueService : IService
    {
        void Create(VenueDto dto);

        void Delete(VenueDto dto);

        void Update(VenueDto dto);
    }
}