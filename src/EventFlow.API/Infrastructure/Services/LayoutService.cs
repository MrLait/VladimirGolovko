using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services
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
        public LayoutService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task CreateAsync(LayoutDto dto)
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
            await DbContext.Layouts.CreateAsync(layout);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(LayoutDto dto)
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

            await DbContext.Layouts.DeleteAsync(new Layout { Id = dto.Id });
        }

        /// <inheritdoc/>
        public IEnumerable<LayoutDto> GetAll()
        {
            var layouts = DbContext.Layouts.GetAllAsQueryable();
            List<LayoutDto> layoutDto = new List<LayoutDto>();
            foreach (var layout in layouts)
            {
                layoutDto.Add(new LayoutDto
                {
                    Id = layout.Id, Description = layout.Description, VenueId = layout.VenueId,
                });
            }

            return layoutDto;
        }

        /// <inheritdoc/>
        public async Task<LayoutDto> GetByIDAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var layout = await DbContext.Layouts.GetByIDAsync(id);
            var layoutDto = new LayoutDto
            {
                Id = layout.Id,
                Description = layout.Description,
                VenueId = layout.VenueId,
            };

            return layoutDto;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(LayoutDto dto)
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

            await DbContext.Layouts.UpdateAsync(new Layout { Id = dto.Id, VenueId = dto.VenueId, Description = dto.Description });
        }

        private bool CheckThatLayoutForThisVenueWithDescriptionAlreadyExist(LayoutDto dto)
        {
            var allLayoutByVenueId = DbContext.Layouts.GetAllAsQueryable().Where(x => x.VenueId == dto.VenueId).ToList();
            var isLayoutContain = allLayoutByVenueId.Any(x => x.Description.Contains(dto.Description));
            return isLayoutContain;
        }
    }
}
