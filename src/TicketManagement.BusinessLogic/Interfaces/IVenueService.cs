using System.Collections.Generic;
using TicketManagement.BusinessLogic.DTO;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IVenueService
    {
        void CreateVenue(VenueDto venueDto);

        VenueDto GetVenue(int? id);

        IEnumerable<VenueDto> GetVenue();
    }
}
