using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Interfaces
{
    public interface IVenueService : IService
    {
        public void Create(VenueDto dto);

        public void Delete(VenueDto dto);

        public void Update(VenueDto dto);
    }
}