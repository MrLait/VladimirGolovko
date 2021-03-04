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

        public override void Create(EventDto dto)
        {
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
            List<Seat> allSeatsForAllAreas = new List<Seat>();
            foreach (var item in allAreasInLayout)
            {
                isAreasEmpty.Add(allSeats.Where(x => x.AreaId == item.Id).Select(x => x.AreaId.ToString().Contains(item.Id.ToString())).Where(z => z.Equals(true)).ElementAtOrDefault(0));
                allSeatsForAllAreas.AddRange(allSeats.Where(x => x.AreaId == item.Id));
            }

            if (isAreasEmpty.Contains(false))
            {
                throw new ValidationException($"Can not create the Event {dto.Description} because one of the Area has no seats.");
            }

            Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime };
            DbContext.Events.Create(eventEntity);
            var incrementedEventId = allEvents.Last().Id + 1;

            foreach (var item in allAreasInLayout)
            {
                DbContext.EventAreas.Create(new EventArea { Description = item.Description, EventId = incrementedEventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = dto.Price });
            }

            foreach (var item in allSeatsForAllAreas)
            {
                DbContext.EventSeats.Create(new EventSeat { EventAreaId = item.AreaId, Number = item.Number, Row = item.Row, State = dto.State });
            }
        }

        public override void Delete(EventDto dto)
        {
            var eventId = dto.Id;
            if (eventId == 0)
            {
                throw new ValidationException($"The event id can't be equal zero: {dto.Id}");
            }

            var allEvents = DbContext.Events.GetByID(eventId);

            if (allEvents == null)
            {
                throw new ValidationException($"The event by id: {eventId} does not exist in the store.");
            }

            var allEventAreasForTheEvent = DbContext.EventAreas.GetAll().Where(x => x.EventId == eventId);

            var allEventSeats = DbContext.EventSeats.GetAll().ToList();
            List<EventSeat> allEventSeatsForTheEventArea = new List<EventSeat>();
            foreach (var item in allEventAreasForTheEvent)
            {
                allEventSeatsForTheEventArea.AddRange(allEventSeats.Where(x => x.EventAreaId == item.Id));
            }

            foreach (var item in allEventSeatsForTheEventArea)
            {
                DbContext.EventSeats.Delete(item);
            }

            foreach (var item in allEventAreasForTheEvent)
            {
                DbContext.EventAreas.Delete(item);
            }

            DbContext.Events.Delete(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
        }

        public override void Update(EventDto dto)
        {
            var eventId = dto.Id;
            if (eventId == 0)
            {
                throw new ValidationException($"The event id can't be equal zero: {dto.Id}");
            }

            var allEvents = DbContext.Events.GetByID(eventId);

            if (allEvents == null)
            {
                throw new ValidationException($"The event by id: {eventId} does not exist in the store.");
            }

            DbContext.Events.Update(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
        }
    }
}
