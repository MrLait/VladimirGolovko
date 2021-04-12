using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Event service class.
    /// </summary>
    internal class EventService : IEventService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public EventService(IDbContext dbContext) => DbContext = dbContext;

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        /// <inheritdoc/>
        public void Create(EventDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            bool isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);

            if (!isDataTimeValid)
            {
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.DateTime);
            }

            bool isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeForVenue(dto);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.DateTime);
            }

            var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);

            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
            }

            GetAllAreasInLayoutAndAllSeatsForThisAreas(dto, out IEnumerable<Area> allAreasInLayout, out List<Seat> allSeatsForAllAreas);

            Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime };
            DbContext.Events.Create(eventEntity);

            var incrementedEventId = DbContext.Events.GetAll().Last().Id;

            CreateEventAreasAndThenEventSeats(dto, allAreasInLayout, allSeatsForAllAreas, incrementedEventId);
        }

        /// <inheritdoc/>
        public void Delete(EventDto dto)
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

            var allEvents = DbContext.Events.GetByID(dto.Id);

            if (allEvents == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            DbContext.Events.Delete(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
        }

        /// <inheritdoc/>
        public void Update(EventDto dto)
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

            var curEvent = DbContext.Events.GetByID(dto.Id);
            if (curEvent == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            var isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);
            if (!isDataTimeValid)
            {
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.DateTime);
            }

            var isLayoutChanged = dto.LayoutId != curEvent.LayoutId;
            if (isLayoutChanged)
            {
                var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);
                if (!atLeastOneAreaContainsSeats)
                {
                    throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
                }

                var isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeForVenue(dto);
                if (isEventContainSameVenueInSameTime)
                {
                    throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.DateTime);
                }

                GetAllAreasInLayoutAndAllSeatsForThisAreas(dto, out IEnumerable<Area> allAreasInLayout, out List<Seat> allSeatsForAllAreas);

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

        private bool CheckThatEventNotCreatedInTheSameTimeForVenue(EventDto dto)
        {
            List<Event> allEvents = DbContext.Events.GetAll().ToList();
            var isEventContainSameVenueInSameTime = allEvents.Any(x => x.DateTime.ToString().Contains(dto.DateTime.ToString()) && x.LayoutId == dto.LayoutId);
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

            var atLeastOneAreaContainsSeats = allSeats.Join(allAreasInLayout,
                            seatAreaId => seatAreaId.AreaId,
                            areaId => areaId.Id,
                            (seatAreaId, areaId) => (SeatAreaId: seatAreaId, AreaId: areaId)).Any(x => x.SeatAreaId.AreaId.ToString().Contains(x.AreaId.Id.ToString()));

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
