using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task CreateAsync(EventDto dto)
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

            bool isEventContainSameVenueInSameTime = await CheckThatEventNotCreatedInTheSameTimeForVenueAsync(dto);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.DateTime);
            }

            var atLeastOneAreaContainsSeats = await CheckThatAtLeastOneAreaContainsSeatsAsync(dto);

            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
            }

            IEnumerable<Area> allAreasInLayout = await GetAllAreasInLayoutAsync(dto);
            List<Seat> allSeatsForAllAreas = await GetAllAllSeatsForThisAreasAsync(allAreasInLayout);

            Event eventEntity = new Event { LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime };
            await DbContext.Events.CreateAsync(eventEntity);

            var incrementedEventId = (await DbContext.Events.GetAllAsync()).Last().Id;

            await CreateEventAreasAndThenEventSeatsAsync(dto, allAreasInLayout, allSeatsForAllAreas, incrementedEventId);
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

            await DbContext.Events.DeleteAsync(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
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
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.DateTime);
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
                    throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.DateTime);
                }

                IEnumerable<Area> allAreasInLayout = await GetAllAreasInLayoutAsync(dto);
                List<Seat> allSeatsForAllAreas = await GetAllAllSeatsForThisAreasAsync(allAreasInLayout);

                await DbContext.Events.UpdateAsync(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
                await CreateEventAreasAndThenEventSeatsAsync(dto, allAreasInLayout, allSeatsForAllAreas, dto.Id);
            }
            else
            {
                await DbContext.Events.UpdateAsync(new Event { Id = dto.Id, LayoutId = dto.LayoutId, Description = dto.Description, Name = dto.Name, DateTime = dto.DateTime });
            }
        }

        private async Task<List<Seat>> GetAllAllSeatsForThisAreasAsync(IEnumerable<Area> allAreasInLayout)
        {
            var allSeats = await DbContext.Seats.GetAllAsync();
            var allSeatsForAllAreas = new List<Seat>();
            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange(allSeats.Where(x => x.AreaId == item.Id));
            }

            return allSeatsForAllAreas;
        }

        private async Task<IEnumerable<Area>> GetAllAreasInLayoutAsync(EventDto dto)
        {
            var allAreasInLayout = (await DbContext.Areas.GetAllAsync()).Where(x => x.LayoutId == dto.LayoutId);

            return allAreasInLayout;
        }

        private async Task<bool> CheckThatEventNotCreatedInTheSameTimeForVenueAsync(EventDto dto)
        {
            List<Event> allEvents = (await DbContext.Events.GetAllAsync()).ToList();
            var isEventContainSameVenueInSameTime = allEvents.Any(x => x.DateTime.ToString().Contains(dto.DateTime.ToString()) && x.LayoutId == dto.LayoutId);
            return isEventContainSameVenueInSameTime;
        }

        private bool CheckThatEventNotCreatedInThePast(EventDto dto)
        {
            bool isDataTimeValid = dto.DateTime > DateTime.Now;
            return isDataTimeValid;
        }

        private async Task<bool> CheckThatAtLeastOneAreaContainsSeatsAsync(EventDto dto)
        {
            var allSeats = await DbContext.Seats.GetAllAsync();
            IEnumerable<Area> allAreasInLayout = (await DbContext.Areas.GetAllAsync()).Where(x => x.LayoutId == dto.LayoutId);

            var atLeastOneAreaContainsSeats = allSeats.Join(allAreasInLayout,
                            seatAreaId => seatAreaId.AreaId,
                            areaId => areaId.Id,
                            (seatAreaId, areaId) => (SeatAreaId: seatAreaId, AreaId: areaId)).Any(x => x.SeatAreaId.AreaId.ToString().Contains(x.AreaId.Id.ToString()));

            return atLeastOneAreaContainsSeats;
        }

        private async Task CreateEventAreasAndThenEventSeatsAsync(EventDto dto, IEnumerable<Area> allAreasInLayout, List<Seat> allSeatsForAllAreas, int eventId)
        {
            var lastEventAreaId = (await DbContext.EventAreas.GetAllAsync()).Last().Id;
            foreach (var item in allAreasInLayout)
            {
                await DbContext.EventAreas.CreateAsync(new EventArea { Description = item.Description, EventId = eventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = dto.Price });
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

                await DbContext.EventSeats.CreateAsync(new EventSeat { EventAreaId = lastEventAreaId, Number = item.Number, Row = item.Row, State = dto.State });

                if (currSateId != item.AreaId)
                {
                    isChanged = true;
                    currSateId = item.AreaId;
                }
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
                DateTime = curEvent.DateTime,
                Description = curEvent.Description,
                Name = curEvent.Name,
            };

            return eventDto;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await DbContext.Events.GetAllAsync();
            List<EventDto> eventDto = new List<EventDto>();
            foreach (var item in events)
            {
                eventDto.Add(new EventDto
                {
                    Id = item.Id, LayoutId = item.LayoutId, DateTime = item.DateTime, Description = item.Description, Name = item.Name,
                });
            }

            return eventDto;
        }
    }
}
