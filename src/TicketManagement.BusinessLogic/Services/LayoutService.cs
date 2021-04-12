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
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            bool isLayoutContain = CheckThatLayoutForThisVenueWithDescriptionAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException(ExceptionMessages.LayoutForTheVenueExist, dto.Description);
            }

            Layout layout = new Layout { VenueId = dto.VenueId, Description = dto.Description };
            DbContext.Layouts.Create(layout);
        }

        /// <inheritdoc/>
        public void Delete(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            if (dto.Id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            if (dto.Id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            DbContext.Layouts.Delete(new Layout { Id = dto.Id });
        }

        /// <inheritdoc/>
        public void Update(LayoutDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            if (dto.Id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            if (dto.Id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
            }

            bool isLayoutContain = CheckThatLayoutForThisVenueWithDescriptionAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException(ExceptionMessages.LayoutForTheVenueExist, dto.Description);
            }

            DbContext.Layouts.Update(new Layout { Id = dto.Id, VenueId = dto.VenueId, Description = dto.Description });
        }

        private bool CheckThatLayoutForThisVenueWithDescriptionAlreadyExist(LayoutDto dto)
        {
            var allLayoutByVenueId = DbContext.Layouts.GetAll().Where(x => x.VenueId == dto.VenueId).ToList();
            var isLayoutContain = allLayoutByVenueId.Any(x => x.Description.Contains(dto.Description));
            return isLayoutContain;
        }
    }
}
