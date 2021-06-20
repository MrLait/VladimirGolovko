﻿////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using AutoMapper;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.Extensions.Localization;
////using TicketManagement.BusinessLogic.Infrastructure;
////using TicketManagement.BusinessLogic.Interfaces;
////using TicketManagement.Dto;
////using TicketManagement.Services.EventFlow.API.Models;
////using TicketManagement.Services.EventFlow.API.Services;
////using TicketManagement.Services.EventFlow.API.Services.FileServices;

////namespace TicketManagement.Services.EventFlow.API.Controllers
////{
////    [Authorize(Roles = UserRoles.EventManager)]
////    [Route("api/[controller]")]
////    [ApiController]
////    public class JsonController : ControllerBase
////    {
////        private readonly IJsonSerializerService<ThirdPartyEvent> _jsonSerializerService;
////        private readonly IFileService _fileService;
////        private readonly IMapper _mapper;
////        private readonly IEventService _eventService;
////        private readonly IEventAreaService _eventAreaService;
////        private readonly IStringLocalizer<JsonController> _localizer;

////        public JsonController(IJsonSerializerService<ThirdPartyEvent> jsonSerializerService, IFileService fileService,
////            IMapper mapper, IEventService eventService, IEventAreaService eventAreaService, IStringLocalizer<JsonController> localizer)
////        {
////            _jsonSerializerService = jsonSerializerService;
////            _fileService = fileService;
////            _mapper = mapper;
////            _eventService = eventService;
////            _eventAreaService = eventAreaService;
////            _localizer = localizer;
////        }

////        [HttpGet]
////        public IActionResult ImportJson()
////        {
////            return Ok();
////        }

////        [HttpPost]
////        public IActionResult ImportJson(IFormFile file)
////        {
////            if (file == null)
////            {
////                ModelState.AddModelError(string.Empty, _localizer["Choose a file"]);
////                return Ok();
////            }

////            var fileToString = _fileService.ConvertFileToString(file);
////            var thirdPartyEvents = _jsonSerializerService.DeserializeObjectsFromString(fileToString);
////            var eventsDto = new List<EventViewModel>();

////            foreach (var item in thirdPartyEvents)
////            {
////                eventsDto.Add(new EventViewModel
////                {
////                    Id = item.Id,
////                    Description = item.Description,
////                    EndDateTime = item.EndDate,
////                    ImageUrl = item.PosterImage,
////                    StartDateTime = item.StartDate,
////                    Name = item.Name,
////                });
////            }

////            var vm = new JsonModel
////            {
////                EventItems = eventsDto,
////            };

////            return Ok(vm);
////        }

////        [HttpPost]
////        public async Task<IActionResult> Create(JsonModel indexViewModel)
////        {
////            try
////            {
////                if (indexViewModel.EventItems.All(x => x.EventAreaItems.Count != 0 && x.EventAreaItems.All(x => x.Price > 0)))
////                {
////                    foreach (var item in indexViewModel.EventItems)
////                    {
////                        var eventAreas = _mapper.Map<List<EventAreaItem>, List<EventAreaDto>>(item.EventAreaItems);
////                        await _eventAreaService.UpdatePriceAsync(eventAreas);
////                    }

////                    return RedirectToAction("Index", "EventManager");
////                }

////                for (int i = 0; i < indexViewModel.EventItems.Count; i++)
////                {
////                    if (indexViewModel.EventItems[i].EventAreaItems.Count == 0)
////                    {
////                        var eventDto = _mapper.Map<EventViewModel, EventDto>(indexViewModel.EventItems[i]);
////                        eventDto.ImageUrl = _fileService.SaveImageToFolder(eventDto.ImageUrl);
////                        await _eventService.CreateAsync(eventDto);
////                        var lastAddedEvent = _eventService.Last();
////                        indexViewModel.EventItems[i] = _mapper.Map<EventDto, EventViewModel>(lastAddedEvent);
////                        var eventsDto = _eventAreaService.GetByEventId(lastAddedEvent).ToList();
////                        indexViewModel.EventItems[i].EventAreaItems = _mapper.Map<List<EventAreaDto>, List<EventAreaItem>>(eventsDto);
////                    }
////                }
////            }
////            catch (ValidationException)
////            {
////                ////ModelStateValidation(ve);

////                return BadRequest(indexViewModel);
////            }

////            return BadRequest(indexViewModel);
////        }

////        ////private void ModelStateValidation(ValidationException ve)
////        ////{
////        ////    if (ve.Message == ExceptionMessages.PriceIsZero)
////        ////    {
////        ////        ViewData["PriceRequired"] = _localizer["PriceRequired"];
////        ////    }

////        ////    if (ve.Message == ExceptionMessages.PriceIsNegative)
////        ////    {
////        ////        ViewData["PriceRequired"] = _localizer["PriceRequired"];
////        ////    }

////        ////    if (ve.Message == ExceptionMessages.CantBeCreatedInThePast)
////        ////    {
////        ////        ModelState.AddModelError(string.Empty, _localizer["The event can't be created in the past"]);
////        ////    }

////        ////    if (ve.Message == ExceptionMessages.EventForTheSameVenueInTheSameDateTime)
////        ////    {
////        ////        ModelState.AddModelError(string.Empty, _localizer["Event for the Venue with the dateTime already exist"]);
////        ////    }

////        ////    if (ve.Message == ExceptionMessages.StartDataTimeBeforeEndDataTime)
////        ////    {
////        ////        ModelState.AddModelError(string.Empty, _localizer["The beginning of the event cannot be after the end of the event"]);
////        ////    }

////        ////    if (ve.Message == ExceptionMessages.ThereIsNoSuchLayout)
////        ////    {
////        ////        ModelState.AddModelError(string.Empty, _localizer["There is no such layout"]);
////        ////    }

////        ////    if (ve.Message == ExceptionMessages.ThereAreNoSeatsInTheEvent)
////        ////    {
////        ////        ModelState.AddModelError(string.Empty, _localizer["There are no seats in the event"]);
////        ////    }
////        ////}
////    }
////}