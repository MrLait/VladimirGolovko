using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.DTO;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal class VenueService : IVenueService
    {
        public VenueService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IDbContext DbContext { get; set; }

        public VenueDto GetVenue(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VenueDto> GetVenue()
        {
            throw new NotImplementedException();
        }

        public void CreateVenue(VenueDto venueDto)
        {
            var allVenues = DbContext.Venues.GetAll().ToList();

            if (allVenues != null)
            {
                var isContain = allVenues.Select(x => x.Description.Contains(venueDto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

                if (isContain)
                {
                    return;
                }
            }

            throw new NotImplementedException();
        }
    }
}
