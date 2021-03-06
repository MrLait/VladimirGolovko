using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class VenueService : AbstractService<VenueDto>
    {
        public VenueService(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public override void Create(VenueDto dto)
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

        public override void Delete(VenueDto dto)
        {
            DbContext.Venues.Delete(new Venue { Id = dto.Id });
        }

        public override void Update(VenueDto dto)
        {
            DbContext.Venues.Update(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }
    }
}
