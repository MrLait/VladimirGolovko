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
        private readonly IEventAreaService _eventAreaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="eventAreaService">Event area service.</param>
        public EventService(IDbContext dbContext, IEventAreaService eventAreaService)
        {
            DbContext = dbContext;
            _eventAreaService = eventAreaService;
        }

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

            Event eventEntity = new Event
            {
                LayoutId = dto.LayoutId,
                Description = dto.Description,
                Name = dto.Name,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                ImageUrl = dto.ImageUrl,
            };
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

            await DbContext.Events.DeleteAsync(new Event
            {
                Id = dto.Id,
            });
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

            var isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);
            if (!isDataTimeValid)
            {
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.StartDateTime);
            }

            await DbContext.Events.UpdateAsync(new Event
            {
                Id = dto.Id,
                LayoutId = dto.LayoutId,
                Description = dto.Description,
                Name = dto.Name,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                ImageUrl = dto.ImageUrl,
            });
        }

        public async Task UpdateLayoutIdAsync(EventDto dto)
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

            var isDataTimeValid = CheckThatEventNotCreatedInThePast(dto);
            if (!isDataTimeValid)
            {
                throw new ValidationException(ExceptionMessages.EventDateTimeValidation, dto.StartDateTime);
            }

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

            var allEventAreasInEvent = await _eventAreaService.GetAllEventAreasForEventAsync(dto);
            var allAreasInLayout = await GetAllAreasInLayoutAsync(dto);
            var allSeatsForAllAreas = await GetAllSeatsForThisAreasAsync(allAreasInLayout);
            foreach (var item in allEventAreasInEvent)
            {
                await _eventAreaService.DeleteAsync(item);
            }

            await DbContext.Events.UpdateAsync(
                new Event
                {
                    Id = dto.Id,
                    LayoutId = dto.LayoutId,
                    Description = dto.Description,
                    Name = dto.Name,
                    StartDateTime = dto.StartDateTime,
                    EndDateTime = dto.EndDateTime,
                    ImageUrl = dto.ImageUrl,
                });
            await CreateEventAreasAndThenEventSeatsAsync(allAreasInLayout, allSeatsForAllAreas, dto.Id);
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

                if (allEventAreasInLayout.Count() != 0)
                {
                    var allEventSeatsForAllAreas = (await GetAllEventSeatsForThisEventAreasAsync(allEventAreasInLayout)).ToList();

                    if (allEventSeatsForAllAreas.Count != 0)
                    {
                        var avalibleSeats = allEventSeatsForAllAreas.Where(x => x.State == States.Available);
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
                }
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
            foreach (var item in allAreasInLayout)
            {
                await DbContext.EventAreas.CreateAsync(new EventArea { Description = item.Description, EventId = eventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = default });
            }

            var lastEventAreaId = ((await DbContext.EventAreas.GetAllAsync()).LastOrDefault()?.Id ?? 0) - allAreasInLayout.Count();

            int currSateId = allSeatsForAllAreas.FirstOrDefault()?.AreaId ?? 0;
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
