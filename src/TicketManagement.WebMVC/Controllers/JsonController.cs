using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.Services.FileServices;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize(Roles = "eventManager")]
    public class JsonController : Controller
    {
        private readonly IJsonSerializerService<ThirdPartyEvent> _jsonSerializerService;
        private readonly IFileService _fileService;

        public JsonController(IJsonSerializerService<ThirdPartyEvent> jsonSerializerService, IFileService fileService)
        {
            _jsonSerializerService = jsonSerializerService;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult ImportJson()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportJson(IFormFile file)
        {
            var fileToString = _fileService.ConvertFileToString(file);
            var thirdPartyEvents = _jsonSerializerService.DeserializeObjectsFromString(fileToString);
            var eventsDto = new List<EventDto>();

            foreach (var item in thirdPartyEvents)
            {
                eventsDto.Add(new EventDto
                {
                    Description = item.Description,
                    EndDateTime = item.EndDate,
                    ImageUrl = item.PosterImage,
                    StartDateTime = item.StartDate,
                    Name = item.Name,
                });
            }

            var vm = new IndexViewModel
            {
                EventItems = eventsDto,
            };

            return View(vm);
        }
    }
}
