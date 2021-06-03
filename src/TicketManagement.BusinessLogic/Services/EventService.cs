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
        private readonly IEventSeatService _eventSeatService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public EventService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="eventSeatService">Event seat service.</param>
        /// <param name="eventAreaService">Event area service.</param>
        public EventService(IDbContext dbContext, IEventSeatService eventSeatService, IEventAreaService eventAreaService)
            : this(dbContext)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
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
                throw new ValidationException(ExceptionMessages.CantBeCreatedInThePast);
            }

            var isStartDataTimeBeforeEndTadaTime = CheckThatStartDataTimeBeforeEndDadaTime(dto);
            if (!isStartDataTimeBeforeEndTadaTime)
            {
                throw new ValidationException(ExceptionMessages.StartDataTimeBeforeEndDataTime);
            }

            bool isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeForVenue(dto);
            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime);
            }

            var atLeastOneLayoutIdExistInArea = CheckThatLayoutIdExistAtLeastInOneArea(dto);
            if (!atLeastOneLayoutIdExistInArea)
            {
                throw new ValidationException(ExceptionMessages.ThereIsNoSuchLayout);
            }

            var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);
            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent);
            }

            var allAreasInLayout = GetAllAreasInLayout(dto);
            var allSeatsForAllAreas = GetAllSeatsForThisAreas(allAreasInLayout);

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

            var allEvent = DbContext.Events.GetAllAsQueryable().AsEnumerable();
            var incrementedEventId = allEvent.Any()? allEvent.Last().Id : 1;

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

            var allEventAreas = _eventAreaService.GetByEventId(dto).ToList();
            bool isSeatPurchased = false;
            foreach (var item in allEventAreas)
            {
                var eventSeats = _eventSeatService.GetByEventAreaId(item);
                isSeatPurchased = eventSeats.Any(x => x.State == States.Purchased);
                if (isSeatPurchased)
                {
                    throw new ValidationException(ExceptionMessages.SeatsHaveAlreadyBeenPurchased, dto.Id);
                }
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
                throw new ValidationException(ExceptionMessages.CantBeCreatedInThePast);
            }

            var isStartDataTimeBeforeEndTadaTime = CheckThatStartDataTimeBeforeEndDadaTime(dto);
            if (!isStartDataTimeBeforeEndTadaTime)
            {
                throw new ValidationException(ExceptionMessages.StartDataTimeBeforeEndDataTime);
            }

            var isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeForVenue(dto);
            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.StartDateTime);
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
                throw new ValidationException(ExceptionMessages.CantBeCreatedInThePast);
            }

            var atLeastOneLayoutIdExistInArea = CheckThatLayoutIdExistAtLeastInOneArea(dto);
            if (!atLeastOneLayoutIdExistInArea)
            {
                throw new ValidationException(ExceptionMessages.ThereIsNoSuchLayout);
            }

            var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);
            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent);
            }

            var isStartDataTimeBeforeEndTadaTime = CheckThatStartDataTimeBeforeEndDadaTime(dto);
            if (!isStartDataTimeBeforeEndTadaTime)
            {
                throw new ValidationException(ExceptionMessages.StartDataTimeBeforeEndDataTime);
            }

            var isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeForVenue(dto);
            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.StartDateTime);
            }

            var allEventAreasInEvent = _eventAreaService.GetAllEventAreasForEvent(dto);
            var allAreasInLayout = GetAllAreasInLayout(dto);
            var allSeatsForAllAreas = GetAllSeatsForThisAreas(allAreasInLayout);
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

        public EventDto Last()
        {
            var item = DbContext.Events.GetAllAsQueryable().AsEnumerable().Last();
            return new EventDto
            {
                Id = item.Id,
                LayoutId = item.LayoutId,
                StartDateTime = item.StartDateTime,
                EndDateTime = item.EndDateTime,
                ImageUrl = item.ImageUrl,
                Description = item.Description,
                Name = item.Name,
            };
        }

        /// <inheritdoc/>
        public IEnumerable<EventDto> GetAll()
        {
            var events = DbContext.Events.GetAllAsQueryable().ToList();
            var eventDto = new List<EventDto>();
            foreach (var item in events)
            {
                var allEventAreasInLayout = GetAllEventAreasInLayout(new EventDto { Id = item.Id });

                if (allEventAreasInLayout.Count() != 0)
                {
                    var allEventSeatsForAllAreas = GetAllEventSeatsForThisEventAreas(allEventAreasInLayout).ToList();

                    if (allEventSeatsForAllAreas.Count != 0)
                    {
                        var avalibleSeats = allEventSeatsForAllAreas.Where(x => x.State == (int)States.Available);
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

        private IQueryable<EventArea> GetAllEventAreasInLayout(EventDto dto)
        {
            var allEventAreasInLayout = DbContext.EventAreas.GetAllAsQueryable().Where(x => x.EventId == dto.Id);
            return allEventAreasInLayout;
        }

        private List<EventSeat> GetAllEventSeatsForThisEventAreas(IEnumerable<EventArea> allEventAreasInEvent)
        {
            var allEventSeats = DbContext.EventSeats.GetAllAsQueryable();
            var allEventSeatsForAllEventAreas = new List<EventSeat>();
            foreach (var item in allEventAreasInEvent.ToList())
            {
                allEventSeatsForAllEventAreas.AddRange(allEventSeats.Where(x => x.EventAreaId == item.Id));
            }

            return allEventSeatsForAllEventAreas;
        }

        private IEnumerable<Area> GetAllAreasInLayout(EventDto dto)
        {
            var allAreasInLayout = DbContext.Areas.GetAllAsQueryable().Where(x => x.LayoutId == dto.LayoutId);

            return allAreasInLayout;
        }

        private IEnumerable<Seat> GetAllSeatsForThisAreas(IEnumerable<Area> allAreasInLayout)
        {
            var allSeats = DbContext.Seats.GetAllAsQueryable().ToList();
            var allSeatsForAllAreas = new List<Seat>();
            foreach (var item in allAreasInLayout.ToList())
            {
                allSeatsForAllAreas.AddRange(allSeats.Where(x => x.AreaId == item.Id));
            }

            return allSeatsForAllAreas;
        }

        private bool CheckThatEventNotCreatedInTheSameTimeForVenue(EventDto dto)
        {
            var allEvents = DbContext.Events.GetAllAsQueryable().ToList();
            var isEventContainSameVenueInSameTime = false;
            foreach (var item in allEvents)
            {
                if (item.StartDateTime != dto.StartDateTime && item.EndDateTime != dto.EndDateTime)
                {
                    var isdtoStartTimeBetwenItem = item.StartDateTime <= dto.StartDateTime && dto.StartDateTime <= item.EndDateTime && item.LayoutId == dto.LayoutId;
                    var isEndDtoTimeBetwenItem = dto.StartDateTime <= item.StartDateTime && item.StartDateTime <= dto.EndDateTime && item.LayoutId == dto.LayoutId;
                    var isEndDtoOrStartDtoEqualItem = dto.StartDateTime == item.StartDateTime | item.StartDateTime == dto.EndDateTime && item.LayoutId == dto.LayoutId;
                    isEventContainSameVenueInSameTime = isdtoStartTimeBetwenItem | isEndDtoTimeBetwenItem | isEndDtoOrStartDtoEqualItem;
                }
            }

            return isEventContainSameVenueInSameTime;
        }

        private bool CheckThatStartDataTimeBeforeEndDadaTime(EventDto dto)
        {
            var isStartDataTimeBeforeEndDataTime = dto.StartDateTime < dto.EndDateTime;
            return isStartDataTimeBeforeEndDataTime;
        }

        private bool CheckThatEventNotCreatedInThePast(EventDto dto)
        {
            bool isDataTimeValid = dto.StartDateTime > DateTime.Now;
            return isDataTimeValid;
        }

        private bool CheckThatLayoutIdExistAtLeastInOneArea(EventDto dto)
        {
            var layoutIdExistAtLeastInOneArea = DbContext.Areas.GetAllAsQueryable().Any(x => x.LayoutId == dto.LayoutId);

            return layoutIdExistAtLeastInOneArea;
        }

        private bool CheckThatAtLeastOneAreaContainsSeats(EventDto dto)
        {
            var allSeats = DbContext.Seats.GetAllAsQueryable().ToList();
            var allAreasInLayout = DbContext.Areas.GetAllAsQueryable().Where(x => x.LayoutId == dto.LayoutId);

            var atLeastOneAreaContainsSeats = allSeats.Join(allAreasInLayout,
                            seatAreaId => seatAreaId.AreaId,
                            areaId => areaId.Id,
                            (seatAreaId, areaId) => (SeatAreaId: seatAreaId, AreaId: areaId)).Any(x => x.SeatAreaId.AreaId.ToString().Contains(x.AreaId.Id.ToString()));

            return atLeastOneAreaContainsSeats;
        }

        private async Task CreateEventAreasAndThenEventSeatsAsync(IEnumerable<Area> allAreasInLayout, IEnumerable<Seat> allSeatsForAllAreas, int eventId)
        {
            foreach (var item in allAreasInLayout.ToList())
            {
                await DbContext.EventAreas.CreateAsync(new EventArea { Description = item.Description, EventId = eventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = default });
            }

            var lastEventAreaId = (DbContext.EventAreas.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault()?.Id ?? 0) - allAreasInLayout.Count();

            int currSateId = allSeatsForAllAreas.FirstOrDefault()?.AreaId ?? 0;
            foreach (var item in allSeatsForAllAreas)
            {
                if (currSateId != item.AreaId)
                {
                    currSateId = item.AreaId;
                    lastEventAreaId++;
                }

                await DbContext.EventSeats.CreateAsync(new EventSeat { EventAreaId = lastEventAreaId + 1, Number = item.Number, Row = item.Row, State = (int)States.Available });
            }
        }
    }
}
