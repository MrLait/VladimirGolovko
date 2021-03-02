using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class EventService : AbstractService<EventDto>
    {
        public EventService(IDbContext dbContext)
            : base(dbContext)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Ожидание>")]
        public override void Create(EventDto dto)
        {
            bool isDataTimeValid = dto.DateTime > DateTime.Now;

            if (!isDataTimeValid)
            {
                throw new ValidationException($"The Event with date time: {dto.DateTime} - can't be created in the past.");
            }

            var allEvents = DbContext.Events.GetAll().ToList();
            var isEventContainSameVenueInSameTime = allEvents.Select(x => x.DateTime.ToString().Contains(dto.DateTime.ToString())).Where(z => z.Equals(true)).ElementAtOrDefault(0);
          ////  var isEvenContainSeat = allEvents.Select(x => x.LayoutId = dto.LayoutId);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException($"The Layout {dto.LayoutId} for the Venue with this date time: {dto.DateTime} - already exists.");
            }
            else
            {
                Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime };
                DbContext.Events.Create(eventEntity);
            }
        }
    }
}
