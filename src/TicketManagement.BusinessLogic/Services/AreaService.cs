using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class AreaService : AbstractService<AreaDto>
    {
        public AreaService(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public override void Create(AreaDto dto)
        {
            var allAreas = DbContext.Areas.GetAll().ToList();
            var isLayoutContain = allAreas.Select(x => x.Description.Contains(dto.Description)).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isLayoutContain)
            {
                throw new ValidationException($"The Area for this Layout with the description: {dto.Description} - already exists.");
            }
            else
            {
                Area area = new Area { LayoutId = dto.LayoutId, Description = dto.Description, CoordX = dto.CoordX, CoordY = dto.CoordY };
                DbContext.Areas.Create(area);
            }
        }

        public override void Delete(AreaDto dto)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(AreaDto dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
