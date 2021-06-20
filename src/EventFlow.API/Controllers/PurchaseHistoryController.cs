using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.DataAccess.Enums;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IBasketService _basketService;
        ////private readonly IApplicationUserService _applicationUserService;
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IEventSeatService _eventSeatService;
        ////private readonly IIdentityParser<ApplicationUser> _identityParser;
        ////private readonly IStringLocalizer<BasketController> _localizer;

        public PurchaseHistoryController(IBasketService basketService,
        IEventSeatService eventSeatService,
        IPurchaseHistoryService purchaseHistoryService)
        ////IApplicationUserService applicationUserService,
        ////IIdentityParser<ApplicationUser> identityParser,
        ////IStringLocalizer<BasketController> localizer
        {
            _basketService = basketService;
            ////_applicationUserService = applicationUserService;
            _purchaseHistoryService = purchaseHistoryService;
            _eventSeatService = eventSeatService;
            ////_identityParser = identityParser;
            ////_localizer = localizer;
        }

        [HttpGet("addItem")]
        public async Task<IActionResult> AddItemAsync(string userId, int itemId)
        {
            await _purchaseHistoryService.AddAsync(userId, itemId);
            await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Purchased });
            return Ok();
        }

        [HttpGet("getAllByUserId")]
        public async Task<IActionResult> GetAllByUserIdAsync(string userId)
        {
            var vm = await _purchaseHistoryService.GetAllByUserIdAsync(userId);
            return Ok(vm);
        }
    }
}
