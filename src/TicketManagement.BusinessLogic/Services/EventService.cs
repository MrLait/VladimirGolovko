using System;
using System.Collections.Generic;
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
            /////utc
            bool isDataTimeValid = dto.DateTime > DateTime.Now;

            if (!isDataTimeValid)
            {
                throw new ValidationException($"The Event with date time: {dto.DateTime} - can't be created in the past.");
            }

            var allEvents = DbContext.Events.GetAll().ToList();
            var isEventContainSameVenueInSameTime = allEvents.Select(x => x.DateTime.ToString().Contains(dto.DateTime.ToString())).Where(z => z.Equals(true)).ElementAtOrDefault(0);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException($"Can not create the Event {dto.Description} for the Venue with the same date time: {dto.DateTime}");
            }

            var allSeats = DbContext.Seats.GetAll();
            var allAreasInLayout = DbContext.Areas.GetAll().Where(x => x.LayoutId == dto.LayoutId);
            List<bool> isAreasEmpty = new List<bool>();
            foreach (var item in allAreasInLayout)
            {
                isAreasEmpty.Add(allSeats.Where(x => x.AreaId == item.Id).Select(x => x.AreaId.ToString().Contains(item.Id.ToString())).Where(z => z.Equals(true)).ElementAtOrDefault(0));
            }

            if (isAreasEmpty.Contains(false))
            {
                throw new ValidationException($"Can not create the Event {dto.Description} because one of the Area has no seats.");
            }

            Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime };
            DbContext.Events.Create(eventEntity);
        }
    }
}
