using System;
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
        internal LayoutService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not create null object: {dto}!");
            }

            bool isLayoutContain = ChackThatLayoutForThisVenueWithDescriptionAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Layout for this Venue with the description: {dto.Description} - already exists.");
            }

            Layout layout = new Layout { VenueId = dto.VenueId, Description = dto.Description };
            DbContext.Layouts.Create(layout);
        }

        public void Delete(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not delete null object: {dto}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not delete object with id: {dto.Id}!");
            }

            DbContext.Layouts.Delete(new Layout { Id = dto.Id });
        }

        public void Update(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {dto}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            bool isLayoutContain = ChackThatLayoutForThisVenueWithDescriptionAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Layout for this Venue with the description: {dto.Description} - already exists.");
            }

            DbContext.Layouts.Update(new Layout { Id = dto.Id, VenueId = dto.VenueId, Description = dto.Description });
        }

        private bool ChackThatLayoutForThisVenueWithDescriptionAlreadyExist(LayoutDto dto)
        {
            var allLayoutByVenueId = DbContext.Layouts.GetAll().Where(x => x.VenueId == dto.VenueId).ToList();
            var isLayoutContain = allLayoutByVenueId.Select(x => x.Description.Contains(dto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);
            return isLayoutContain;
        }
    }
}
