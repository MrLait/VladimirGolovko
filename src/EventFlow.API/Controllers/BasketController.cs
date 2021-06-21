using System.Threading.Tasks;
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
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IEventSeatService _eventSeatService;

        public BasketController(IBasketService basketService,
        IEventSeatService eventSeatService)
        {
            _basketService = basketService;
            _eventSeatService = eventSeatService;
        }

        /// <summary>
        /// Get all items from user basket.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Returns basket items.</returns>
        [HttpGet("getAllByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByUserIdAsync(string id)
        {
            var basketModel = await _basketService.GetAllByUserIdAsync(id);
            return Ok(basketModel);
        }

        /// <summary>
        /// Add item to basket.
        /// </summary>
        /// <param name="model">Add to basket model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPost("addToBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddToBasketAsync([FromBody] AddToBasketModel model)
        {
            await _basketService.AddAsync(model.UserId, model.ItemId);
            await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = model.ItemId, State = States.Booked });

            return Ok();
        }

        /// <summary>
        /// Delete item from basket.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="itemId">Item id.</param>
        /// <returns>Returns status code.</returns>
        [HttpDelete("removeFromBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFromBasketAsync(string userId, int itemId)
        {
            await _basketService.DeleteAsync(userId, itemId);
            await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Available });
            return Ok();
        }

        /// <summary>
        /// Delete all items by user id.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Returns status code.</returns>
        [HttpDelete("deleteAllByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAllByUserIdAsync(string id)
        {
            await _basketService.DeleteAllByUserIdAsync(id);
            return Ok();
        }
    }
}
