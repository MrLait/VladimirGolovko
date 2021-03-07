using System;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class AreaService : IAreaService
    {
        internal AreaService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not create null object: {dto}!");
            }

            bool isLayoutContain = ChackThatAreaWithThisDescriptionInLayoutAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Area for this Layout with the description: {dto.Description} - already exists.");
            }

            Area area = new Area { LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY };
            DbContext.Areas.Create(area);
        }

        public void Delete(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not delete null object: {dto}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not delete object with id: {dto.Id}!");
            }

            DbContext.Areas.Delete(new Area { Id = dto.Id });
        }

        public void Update(AreaDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException($"Can not update null object: {dto}!");
            }

            if (dto.Id <= 0)
            {
                throw new ArgumentException($"Can not update object with id: {dto.Id}!");
            }

            bool isLayoutContain = ChackThatAreaWithThisDescriptionInLayoutAlreadyExist(dto);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Area for this Layout with the description: {dto.Description} - already exists.");
            }

            DbContext.Areas.Update(new Area { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY });
        }

        private bool ChackThatAreaWithThisDescriptionInLayoutAlreadyExist(AreaDto dto)
        {
            var allAreas = DbContext.Areas.GetAll().ToList();
            var isLayoutContain = allAreas.Select(x => x.Description.Contains(dto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);
            return isLayoutContain;
        }
    }
}
