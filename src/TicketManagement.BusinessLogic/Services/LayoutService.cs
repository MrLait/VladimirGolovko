using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class LayoutService : ILayoutService
    {
        public LayoutService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(LayoutDto dto)
        {
            var allLayoutByVenueId = DbContext.Layouts.GetAll().Where(x => x.VenueId == dto.VenueId).ToList();
            var isLayoutContain = allLayoutByVenueId.Select(x => x.Description.Contains(dto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Layout for this Venue with the description: {dto.Description} - already exists.");
            }
            else
            {
                Layout layout = new Layout { VenueId = dto.VenueId, Description = dto.Description };
                DbContext.Layouts.Create(layout);
            }
        }

        public void Delete(LayoutDto dto)
        {
            DbContext.Layouts.Delete(new Layout { Id = dto.Id });
        }

        public void Update(LayoutDto dto)
        {
            DbContext.Layouts.Update(new Layout { Id = dto.Id, VenueId = dto.VenueId, Description = dto.Description });
        }
    }
}
