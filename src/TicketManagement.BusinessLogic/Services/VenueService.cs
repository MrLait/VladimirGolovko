using System;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    public class VenueService : IVenueService
    {
        public VenueService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not create null object: {dto}!");
            }

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
            if (dto == null)
            {
                throw new ArgumentException($"Can not delete null object: {dto}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not delete object with id: {dto.Id}!");
            }

            DbContext.Venues.Delete(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }

        public void Update(VenueDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {dto}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            DbContext.Venues.Update(new Venue { Id = dto.Id, Description = dto.Description, Address = dto.Address, Phone = dto.Phone });
        }
    }
}
