using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketManagement.BusinessLogic.DTO;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal class LayoutService : AbstractService, ILayoutService
    {
        public LayoutService(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public void CreateLayout(LayoutDto layoutDto)
        {
            var allLayoutByVenueId = DbContext.Layouts.GetAll().Where(x => x.VenueId == layoutDto.VenueId).ToList();
            var isLayoutContain = allLayoutByVenueId.Select(x => x.Description.Contains(layoutDto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Layout for this Venue with the description: {layoutDto.Description} - already exists.");
            }
            else
            {
                Layout layout = new Layout { VenueId = layoutDto.VenueId, Description = layoutDto.Description };
                DbContext.Layouts.Create(layout);
            }
        }

        public LayoutDto GetLayout(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LayoutDto> GetLayouts()
        {
            throw new NotImplementedException();
        }
    }
}
