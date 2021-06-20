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
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IEventSeatService _eventSeatService;

        public PurchaseHistoryController(IBasketService basketService,
        IEventSeatService eventSeatService,
        IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
            _eventSeatService = eventSeatService;
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
