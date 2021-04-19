﻿using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
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
        internal AreaService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; private set; }

        /// <inheritdoc/>
        public void Create(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            bool isLayoutContain = CheckThatAreaWithThisDescriptionInLayoutAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException(ExceptionMessages.AreaForTheLayoutExist, dto.Description);
            }

            Area area = new Area { LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY };
            DbContext.Areas.Create(area);
        }

        /// <inheritdoc/>
        public void Delete(AreaDto dto)
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

            DbContext.Areas.Delete(new Area { Id = dto.Id });
        }

        /// <inheritdoc/>
        public IEnumerable<AreaDto> GetAll()
        {
            var areas = DbContext.Areas.GetAll();
            List<AreaDto> areasDto = new List<AreaDto>();
            foreach (var item in areas)
            {
                areasDto.Add(new AreaDto { Id = item.Id, LayoutId = item.LayoutId, Description = item.Description, CoordX = item.CoordX, CoordY = item.CoordY });
            }

            return areasDto;
        }

        /// <inheritdoc/>
        public AreaDto GetByID(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var area = DbContext.Areas.GetByID(id);
            return new AreaDto { Id = area.Id, LayoutId = area.LayoutId, Description = area.Description, CoordX = area.CoordX, CoordY = area.CoordY };
        }

        /// <inheritdoc/>
        public void Update(AreaDto dto)
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

            bool isLayoutContain = CheckThatAreaWithThisDescriptionInLayoutAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException(ExceptionMessages.AreaForTheLayoutExist, dto.Description);
            }

            DbContext.Areas.Update(new Area { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY });
        }

        private bool CheckThatAreaWithThisDescriptionInLayoutAlreadyExist(AreaDto dto)
        {
            var allAreas = DbContext.Areas.GetAll().Where(x => x.LayoutId == dto.LayoutId);
            var isLayoutContain = allAreas.Any(x => x.Description.Contains(dto.Description));
            return isLayoutContain;
        }
    }
}
