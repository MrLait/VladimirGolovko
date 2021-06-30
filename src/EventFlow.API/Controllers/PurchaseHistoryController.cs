using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.DataAccess.Domain.Enums;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    /// <summary>
    /// Purchase history api controller.
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IEventSeatService _eventSeatService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseHistoryController"/> class.
        /// </summary>
        /// <param name="eventSeatService">Event seat service.</param>
        /// <param name="purchaseHistoryService">Purchase history service.</param>
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
        [HttpPost]
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByUserIdAsync(string userId)
        {
            var purchaseHistoryModel = await _purchaseHistoryService.GetAllByUserIdAsync(userId);
            return Ok(purchaseHistoryModel);
        }
    }
}
