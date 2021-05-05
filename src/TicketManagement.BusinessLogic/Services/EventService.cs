using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Enums;
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
        public async Task CreateAsync(EventDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            bool isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);

            if (!isDataTimeValid)
            {
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.StartDateTime);
            }

            bool isEventContainSameVenueInSameTime = await CheckThatEventNotCreatedInTheSameTimeForVenueAsync(dto);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.StartDateTime);
            }

            var atLeastOneAreaContainsSeats = await CheckThatAtLeastOneAreaContainsSeatsAsync(dto);

            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
            }

            var allAreasInLayout = await GetAllAreasInLayoutAsync(dto);
            var allSeatsForAllAreas = await GetAllSeatsForThisAreasAsync(allAreasInLayout);

            Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, StartDateTime = dto.StartDateTime };
            await DbContext.Events.CreateAsync(eventEntity);

            var incrementedEventId = (await DbContext.Events.GetAllAsync()).Last().Id;

            await CreateEventAreasAndThenEventSeatsAsync(allAreasInLayout, allSeatsForAllAreas, incrementedEventId);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(EventDto dto)
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

            var allEvents = await DbContext.Events.GetByIDAsync(dto.Id);

            if (allEvents == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            await DbContext.Events.DeleteAsync(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, StartDateTime = dto.StartDateTime });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(EventDto dto)
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

            var curEvent = await DbContext.Events.GetByIDAsync(dto.Id);
            if (curEvent == null)
            {
                throw new ValidationException(ExceptionMessages.NullReference);
            }

            var isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);
            if (!isDataTimeValid)
            {
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.StartDateTime);
            }

            var isLayoutChanged = dto.LayoutId != curEvent.LayoutId;
            if (isLayoutChanged)
            {
                var atLeastOneAreaContainsSeats = await CheckThatAtLeastOneAreaContainsSeatsAsync(dto);
                if (!atLeastOneAreaContainsSeats)
                {
                    throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
                }

                var isEventContainSameVenueInSameTime = await CheckThatEventNotCreatedInTheSameTimeForVenueAsync(dto);
                if (isEventContainSameVenueInSameTime)
                {
                    throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.StartDateTime);
                }

                var allAreasInLayout = await GetAllAreasInLayoutAsync(dto);
                var allSeatsForAllAreas = await GetAllSeatsForThisAreasAsync(allAreasInLayout);

                await DbContext.Events.UpdateAsync(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, StartDateTime = dto.StartDateTime });
                await CreateEventAreasAndThenEventSeatsAsync(allAreasInLayout, allSeatsForAllAreas, dto.Id);
            }
            else
            {
                await DbContext.Events.UpdateAsync(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, StartDateTime = dto.StartDateTime });
            }
        }

        /// <inheritdoc/>
        public async Task<EventDto> GetByIDAsync(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            if (id < 0)
            {
                throw new ValidationException(ExceptionMessages.IdIsZero, id);
            }

            var curEvent = await DbContext.Events.GetByIDAsync(id);
            var eventDto = new EventDto
            {
                Id = curEvent.Id,
                LayoutId = curEvent.LayoutId,
                StartDateTime = curEvent.StartDateTime,
                Description = curEvent.Description,
                Name = curEvent.Name,
                ImageUrl = curEvent.ImageUrl,
                EndDateTime = curEvent.EndDateTime,
            };

            return eventDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await DbContext.Events.GetAllAsync();
            var eventDto = new List<EventDto>();
            foreach (var item in events)
            {
                var allEventAreasInLayout = await GetAllEvntAreasInLayoutAsync(new EventDto { Id = item.Id });

                if (allEventAreasInLayout.Count() == 0)
                {
                    return eventDto;
                }

                var allEventSeatsForAllAreas = (await GetAllEventSeatsForThisEventAreasAsync(allEventAreasInLayout)).ToList();

                if (allEventSeatsForAllAreas.Count == 0)
                {
                    return eventDto;
                }

                var avalibleSeats = allEventSeatsForAllAreas.Where(x => x.State == 0);
                eventDto.Add(new EventDto
                {
                    Id = item.Id,
                    LayoutId = item.LayoutId,
                    StartDateTime = item.StartDateTime,
                    EndDateTime = item.EndDateTime,
                    ImageUrl = item.ImageUrl,
                    Description = item.Description,
                    Name = item.Name,
                    AvailableSeats = avalibleSeats.Count(),
                    PriceFrom = allEventAreasInLayout.Min(x => x.Price),
                    PriceTo = allEventAreasInLayout.Max(x => x.Price),
                });
            }

            return eventDto;
        }

        private async Task<IQueryable<EventArea>> GetAllEvntAreasInLayoutAsync(EventDto dto)
        {
            var allEventAreasInLayout = (await DbContext.EventAreas.GetAllAsync()).Where(x => x.EventId == dto.Id);

            return allEventAreasInLayout;
        }

        private async Task<List<EventSeat>> GetAllEventSeatsForThisEventAreasAsync(IQueryable<EventArea> allEventAreasInEvent)
        {
            var allEventSeats = await DbContext.EventSeats.GetAllAsync();
            var allEventSeatsForAllEventAreas = new List<EventSeat>();
            foreach (var item in allEventAreasInEvent)
            {
                allEventSeatsForAllEventAreas.AddRange(allEventSeats.Where(x => x.EventAreaId == item.Id));
            }

            return allEventSeatsForAllEventAreas;
        }

        private async Task<IEnumerable<Area>> GetAllAreasInLayoutAsync(EventDto dto)
        {
            var allAreasInLayout = (await DbContext.Areas.GetAllAsync()).Where(x => x.LayoutId == dto.LayoutId);

            return allAreasInLayout;
        }

        private async Task<List<Seat>> GetAllSeatsForThisAreasAsync(IEnumerable<Area> allAreasInLayout)
        {
            var allSeats = await DbContext.Seats.GetAllAsync();
            var allSeatsForAllAreas = new List<Seat>();
            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange(allSeats.Where(x => x.AreaId == item.Id));
            }

            return allSeatsForAllAreas;
        }

        private async Task<bool> CheckThatEventNotCreatedInTheSameTimeForVenueAsync(EventDto dto)
        {
            var allEvents = (await DbContext.Events.GetAllAsync()).ToList();
            var isEventContainSameVenueInSameTime = allEvents.Any(x => x.StartDateTime.ToString().Contains(dto.StartDateTime.ToString()) && x.LayoutId == dto.LayoutId);
            return isEventContainSameVenueInSameTime;
        }

        private bool CheckThatEventNotCreatedInThePast(EventDto dto)
        {
            bool isDataTimeValid = dto.StartDateTime > DateTime.Now;
            return isDataTimeValid;
        }

        private async Task<bool> CheckThatAtLeastOneAreaContainsSeatsAsync(EventDto dto)
        {
            var allSeats = (await DbContext.Seats.GetAllAsync()).ToList();
            var allAreasInLayout = (await DbContext.Areas.GetAllAsync()).Where(x => x.LayoutId == dto.LayoutId);

            var atLeastOneAreaContainsSeats = allSeats.Join(allAreasInLayout,
                            seatAreaId => seatAreaId.AreaId,
                            areaId => areaId.Id,
                            (seatAreaId, areaId) => (SeatAreaId: seatAreaId, AreaId: areaId)).Any(x => x.SeatAreaId.AreaId.ToString().Contains(x.AreaId.Id.ToString()));

            return atLeastOneAreaContainsSeats;
        }

        private async Task CreateEventAreasAndThenEventSeatsAsync(IEnumerable<Area> allAreasInLayout, List<Seat> allSeatsForAllAreas, int eventId)
        {
            var lastEventAreaId = (await DbContext.EventAreas.GetAllAsync()).Last().Id;
            foreach (var item in allAreasInLayout)
            {
                await DbContext.EventAreas.CreateAsync(new EventArea { Description = item.Description, EventId = eventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = default });
            }

            int currSateId = allSeatsForAllAreas.FirstOrDefault().AreaId;
            bool isChanged = true;
            foreach (var item in allSeatsForAllAreas)
            {
                if (isChanged)
                {
                    lastEventAreaId++;
                    isChanged = false;
                }

                await DbContext.EventSeats.CreateAsync(new EventSeat { EventAreaId = lastEventAreaId, Number = item.Number, Row = item.Row, State = States.Available });

                if (currSateId != item.AreaId)
                {
                    isChanged = true;
                    currSateId = item.AreaId;
                }
            }
        }
    }
}
