﻿using System.Collections.Generic;
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
    /// Area service class.
    /// </summary>
    internal class AreaService : IAreaService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public AreaService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task CreateAsync(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            var isLayoutContain = CheckThatAreaWithThisDescriptionInLayoutAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException(ExceptionMessages.AreaForTheLayoutExist, dto.Description);
            }

            var area = new Area { LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY };
            await DbContext.Areas.CreateAsync(area);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            switch (dto.Id)
            {
                case 0:
                    throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
                case < 0:
                    throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
                default:
                    break;
            }

            var isLayoutContain = CheckThatAreaWithThisDescriptionInLayoutAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException(ExceptionMessages.AreaForTheLayoutExist, dto.Description);
            }

            await DbContext.Areas.UpdateAsync(new Area { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY });
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            switch (dto.Id)
            {
                case 0:
                    throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
                case < 0:
                    throw new ValidationException(ExceptionMessages.IdIsZero, dto.Id);
                default:
                    await DbContext.Areas.DeleteAsync(new Area { Id = dto.Id });
                    break;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<AreaDto> GetAll()
        {
            var areas = DbContext.Areas.GetAllAsQueryable();
            var areasDto = new List<AreaDto>();
            foreach (var item in areas)
            {
                areasDto.Add(new AreaDto { Id = item.Id, LayoutId = item.LayoutId, Description = item.Description, CoordX = item.CoordX, CoordY = item.CoordY });
            }

            return areasDto;
        }

        /// <inheritdoc/>
        public async Task<AreaDto> GetByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var area = await DbContext.Areas.GetByIdAsync(id);
            return new AreaDto { Id = area.Id, LayoutId = area.LayoutId, Description = area.Description, CoordX = area.CoordX, CoordY = area.CoordY };
        }

        private bool CheckThatAreaWithThisDescriptionInLayoutAlreadyExist(AreaDto dto)
        {
            var allAreas = DbContext.Areas.GetAllAsQueryable().Where(x => x.LayoutId == dto.LayoutId);
            var isLayoutContain = allAreas.Any(x => x.Description.Contains(dto.Description));
            return isLayoutContain;
        }
    }
}
