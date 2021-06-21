using System.Threading.Tasks;
using AutoMapper;
using EventFlow.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.DataAccess.Enums;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IEventSeatService _eventSeatService;

        public PurchaseHistoryController(IEventSeatService eventSeatService,
        IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
            _eventSeatService = eventSeatService;
        }

        /// <summary>
        /// Add item to purchase history.
        /// </summary>
        /// <param name="model">Purchase history model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPost("addItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddItemAsync([FromBody] AddToPurchaseHistoryModel model)
        {
            await _purchaseHistoryService.AddAsync(model.UserId, model.ItemId);
            await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = model.ItemId, State = States.Purchased });
            return Ok();
        }

        /// <summary>
        /// Get all item from purchase history by user id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Returns purchase history model.</returns>
        [HttpGet("getAllByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByUserIdAsync(string userId)
        {
            var purchaseHistoryModel = await _purchaseHistoryService.GetAllByUserIdAsync(userId);
            return Ok(purchaseHistoryModel);
        }
    }
}
