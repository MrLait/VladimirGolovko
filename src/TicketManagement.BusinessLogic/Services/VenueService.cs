using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class VenueService : IVenueService
    {
        public VenueService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(VenueDto dto)
        {
            var allVenues = DbContext.Venues.GetAll().ToList();
            var isVenueContain = allVenues.Select(x => x.Description.Contains(dto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isVenueContain)
            {
                throw new ValidationException($"The Venue with this description: {dto.Description} - already exists.");
            }
            else
            {
                Venue venue = new Venue { Description = dto.Description, Address = dto.Address, Phone = dto.Phone };
                DbContext.Venues.Create(venue);
            }
        }

        public void Delete(VenueDto dto)
        {
            DbContext.Venues.Delete(new Venue { Id = dto.Id });
        }

        public void Update(VenueDto dto)
        {
            DbContext.Venues.Update(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }
    }
}
