using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize(Roles = "eventManager")]
    public class EventManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;
        private readonly IEventService _eventService;

        public EventManagerController(UserManager<ApplicationUser> applicationUser, IEventService eventService)
        {
            _applicationUserManager = applicationUser;
            _eventService = eventService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ////var user = await ParseAsync(HttpContext.User);

            var eventCatalog = await _eventService.GetAllAsync();
            var vm = new IndexViewModel
            {
                EventItems = eventCatalog,
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = new EventDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    LayoutId = model.LayoutId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    ImageUrl = model.ImageUrl,
                };

                await _eventService.CreateAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEventAsync(int id)
        {
            var eventItem = await _eventService.GetByIDAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            EventViewModel model = new EventViewModel
            {
                Id = id,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EndDateTime = eventItem.EndDateTime,
                ImageUrl = eventItem.ImageUrl,
                LayoutId = eventItem.LayoutId,
                StartDateTime = eventItem.StartDateTime,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = new EventDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    LayoutId = model.LayoutId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    ImageUrl = model.ImageUrl,
                };

                await _eventService.UpdateAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLayoutId(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = new EventDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    LayoutId = model.LayoutId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    ImageUrl = model.ImageUrl,
                };

                await _eventService.UpdateLayoutIdAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            await _eventService.DeleteAsync(new EventDto { Id = id });

            return RedirectToAction("Index", "EventManager");
        }

        public async Task<ApplicationUser> ParseAsync(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                var id = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
                return user;
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
