using System;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Layout service class.
    /// </summary>
    internal class LayoutService : ILayoutService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        internal LayoutService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public void Create(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not create null object: {(LayoutDto) null}!");
            }

            bool isLayoutContain = ChackThatLayoutForThisVenueWithDescriptionAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Layout for this Venue with the description: {dto.Description} - already exists.");
            }

            Layout layout = new Layout { VenueId = dto.VenueId, Description = dto.Description };
            DbContext.Layouts.Create(layout);
        }

        /// <inheritdoc/>
        public void Delete(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not delete null object: {(LayoutDto) null}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not delete object with id: {dto.Id}!");
            }

            DbContext.Layouts.Delete(new Layout { Id = dto.Id });
        }

        /// <inheritdoc/>
        public void Update(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {(LayoutDto) null}!");
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
            var isLayoutContain = allLayoutByVenueId.Any(x => x.Description.Contains(dto.Description));
            return isLayoutContain;
        }
    }
}
