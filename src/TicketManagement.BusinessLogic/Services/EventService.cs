using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    internal class EventService : IEventService
    {
        public EventService(IDbContext dbContext) => DbContext = dbContext;

        public IDbContext DbContext { get; private set; }

        public void Create(EventDto dto)
        {
            IEnumerable<Area> allAreasInLayout;
            List<Seat> allSeatsForAllAreas;

            bool isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);

            if (!isDataTimeValid)
            {
                throw new ValidationException($"The Event with date time: {dto.DateTime} - can't be created in the past.");
            }

            bool isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeFotVenue(dto);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException($"Can not create the Event {dto.Description} for the Venue with the same date time: {dto.DateTime}");
            }

            var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);

            if (atLeastOneAreaContainsSeats)
            {
                throw new ValidationException($"Can not create the Event {dto.Description} because no one of the Area has no seats.");
            }

            GetAllAreasInLayoutAndAllSeatsForThisAreas(dto, out allAreasInLayout, out allSeatsForAllAreas);

            Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime };
            DbContext.Events.Create(eventEntity);

            var incrementedEventId = DbContext.Events.GetAll().Last().Id;

            CreateEventAreasAndThenEventSeats(dto, allAreasInLayout, allSeatsForAllAreas, incrementedEventId);
        }

        public void Delete(EventDto dto)
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

            DbContext.Events.Delete(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
        }

        public void Update(EventDto dto)
        {
            var eventId = dto.Id;
            if (eventId == 0)
            {
                throw new ValidationException($"The event id can't be equal zero: {dto.Id}");
            }

            var curEvent = DbContext.Events.GetByID(eventId);

            if (curEvent == null)
            {
                throw new ValidationException($"The event by id: {eventId} does not exist in the store.");
            }

            var isLayoutChanged = dto.LayoutId != curEvent.LayoutId;

            if (isLayoutChanged)
            {
                IEnumerable<Area> allAreasInLayout;
                List<Seat> allSeatsForAllAreas;

                var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);
                if (atLeastOneAreaContainsSeats)
                {
                    throw new ValidationException($"Can not create the Event {dto.Description} because no one of the Area has no seats.");
                }

                bool isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);
                if (!isDataTimeValid)
                {
                    throw new ValidationException($"The Event with date time: {dto.DateTime} - can't be created in the past.");
                }

                bool isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeFotVenue(dto);
                if (isEventContainSameVenueInSameTime)
                {
                    throw new ValidationException($"Can not create the Event {dto.Description} for the Venue with the same date time: {dto.DateTime}");
                }

                GetAllAreasInLayoutAndAllSeatsForThisAreas(dto, out allAreasInLayout, out allSeatsForAllAreas);

                DbContext.Events.Update(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
                CreateEventAreasAndThenEventSeats(dto, allAreasInLayout, allSeatsForAllAreas, dto.Id);
            }
            else
            {
                DbContext.Events.Update(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
            }
        }

        private void GetAllAreasInLayoutAndAllSeatsForThisAreas(EventDto dto, out IEnumerable<Area> allAreasInLayout, out List<Seat> allSeatsForAllAreas)
        {
            var allSeats = DbContext.Seats.GetAll();
            allAreasInLayout = DbContext.Areas.GetAll().Where(x => x.LayoutId == dto.LayoutId);
            allSeatsForAllAreas = new List<Seat>();
            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange(allSeats.Where(x => x.AreaId == item.Id));
            }
        }

        private bool CheckThatEventNotCreatedInTheSameTimeFotVenue(EventDto dto)
        {
            List<Event> allEvents = DbContext.Events.GetAll().ToList();
            bool isEventContainSameVenueInSameTime = allEvents.Select(x => x.DateTime.ToString().Contains(dto.DateTime.ToString())).Where(z => z.Equals(true)).ElementAtOrDefault(0);
            return isEventContainSameVenueInSameTime;
        }

        private bool CheckThatEventNotCreatedInThePast(EventDto dto)
        {
            bool isDataTimeValid = dto.DateTime > DateTime.Now;
            return isDataTimeValid;
        }

        private bool CheckThatAtLeastOneAreaContainsSeats(EventDto dto)
        {
            var allSeats = DbContext.Seats.GetAll();
            IEnumerable<Area> allAreasInLayout = DbContext.Areas.GetAll().Where(x => x.LayoutId == dto.LayoutId);
            List<bool> isAreasContainSeats = new List<bool>();
            foreach (var item in allAreasInLayout)
            {
                isAreasContainSeats.Add(allSeats.Where(x => x.AreaId == item.Id).Select(x => x.AreaId.ToString().Contains(item.Id.ToString())).Where(z => z.Equals(true)).ElementAtOrDefault(0));
            }

            var atLeastOneAreaContainsSeats = isAreasContainSeats.All(x => x.Equals(false));

            return atLeastOneAreaContainsSeats;
        }

        private void CreateEventAreasAndThenEventSeats(EventDto dto, IEnumerable<Area> allAreasInLayout, List<Seat> allSeatsForAllAreas, int eventId)
        {
            var lastEventAreaId = DbContext.EventAreas.GetAll().Last().Id;
            foreach (var item in allAreasInLayout)
            {
                DbContext.EventAreas.Create(new EventArea { Description = item.Description, EventId = eventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = dto.Price });
            }

            foreach (var item in allSeatsForAllAreas)
            {
                DbContext.EventSeats.Create(new EventSeat { EventAreaId = item.AreaId + lastEventAreaId, Number = item.Number, Row = item.Row, State = dto.State });
            }
        }
    }
}
