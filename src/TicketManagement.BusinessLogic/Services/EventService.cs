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

            bool isEventContainSameVenueInSameTime = CheckThatEventNotCreatedInTheSameTimeForVenue(dto);

            if (isEventContainSameVenueInSameTime)
            {
                throw new ValidationException(ExceptionMessages.EventForTheSameVenueInTheSameDateTime, dto.Description, dto.StartDateTime);
            }

            var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);

            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
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

            var incrementedEventId = DbContext.Events.GetAllAsQueryable().Last().Id;

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

            var atLeastOneAreaContainsSeats = CheckThatAtLeastOneAreaContainsSeats(dto);
            if (!atLeastOneAreaContainsSeats)
            {
                throw new ValidationException(ExceptionMessages.ThereAreNoSeatsInTheEvent, dto.Description);
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

        /// <inheritdoc/>
        public IEnumerable<EventDto> GetAll()
        {
            var events = DbContext.Events.GetAllAsQueryable().ToList();
            var eventDto = new List<EventDto>();
            foreach (var item in events)
            {
                var allEventAreasInLayout = GetAllEvntAreasInLayout(new EventDto { Id = item.Id });

                if (allEventAreasInLayout.Count() != 0)
                {
                    var allEventSeatsForAllAreas = GetAllEventSeatsForThisEventAreas(allEventAreasInLayout).ToList();

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

        private IQueryable<EventArea> GetAllEvntAreasInLayout(EventDto dto)
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

        private List<Seat> GetAllSeatsForThisAreas(IEnumerable<Area> allAreasInLayout)
        {
            var allSeats = DbContext.Seats.GetAllAsQueryable();
            var allSeatsForAllAreas = new List<Seat>();
            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange(allSeats.Where(x => x.AreaId == item.Id));
            }

            return allSeatsForAllAreas;
        }

        private bool CheckThatEventNotCreatedInTheSameTimeForVenue(EventDto dto)
        {
            var allEvents = DbContext.Events.GetAllAsQueryable().ToList();
            var isEventContainSameVenueInSameTime = allEvents.Any(x => x.StartDateTime.ToString().Contains(dto.StartDateTime.ToString()) && x.LayoutId == dto.LayoutId);
            return isEventContainSameVenueInSameTime;
        }

        private bool CheckThatEventNotCreatedInThePast(EventDto dto)
        {
            bool isDataTimeValid = dto.StartDateTime > DateTime.Now;
            return isDataTimeValid;
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

        private async Task CreateEventAreasAndThenEventSeatsAsync(IEnumerable<Area> allAreasInLayout, List<Seat> allSeatsForAllAreas, int eventId)
        {
            foreach (var item in allAreasInLayout)
            {
                await DbContext.EventAreas.CreateAsync(new EventArea { Description = item.Description, EventId = eventId, CoordX = item.CoordX, CoordY = item.CoordY, Price = default });
            }

            var lastEventAreaId = (DbContext.EventAreas.GetAllAsQueryable().LastOrDefault()?.Id ?? 0) - allAreasInLayout.Count();

            int currSateId = allSeatsForAllAreas.FirstOrDefault()?.AreaId ?? 0;
            foreach (var item in allSeatsForAllAreas)
            {
                if (currSateId != item.AreaId)
                {
                    currSateId = item.AreaId;
                    lastEventAreaId++;
                }

                await DbContext.EventSeats.CreateAsync(new EventSeat { EventAreaId = lastEventAreaId + 1, Number = item.Number, Row = item.Row, State = States.Available });
            }
        }
    }
}
