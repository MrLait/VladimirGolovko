using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.DTO;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal class VenueService : AbstractService, IVenueService
    {
        public VenueService(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public VenueDto GetVenue(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VenueDto> GetVenues()
        {
            throw new NotImplementedException();
        }

        public void CreateVenue(VenueDto venueDto)
        {
            var allVenues = DbContext.Venues.GetAll().ToList();
            var isVenueContain = allVenues.Select(x => x.Description.Contains(venueDto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isVenueContain)
            {
                throw new ValidationException($"The Venue with this description: {venueDto.Description} - already exists.");
            }
            else
            {
                Venue venue = new Venue { Description = venueDto.Description, Address = venueDto.Address, Phone = venueDto.Phone };
                DbContext.Venues.Create(venue);
            }
        }
    }
}
