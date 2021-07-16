using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketManagement.DataAccess.Domain.Enums;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    /// <summary>
    /// Basket api Controller.
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IEventSeatService _eventSeatService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketController"/> class.
        /// </summary>
        /// <param name="basketService">Basket service.</param>
        /// <param name="eventSeatService">Event seat service.</param>
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
        [HttpGet]
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
        /// <returns>Returns status code or <see cref="ValidationException"/>.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToBasketAsync([FromBody] AddToBasketModel model)
        {
            try
            {
                await _basketService.AddAsync(model.UserId, model.ItemId);
                await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = model.ItemId, State = States.Booked });

                return Ok();
            }
            catch (ValidationException e)
            {
                var validationExceptionSerialized = JsonConvert.SerializeObject(e);
                return BadRequest(validationExceptionSerialized);
            }
        }

        /// <summary>
        /// Delete item from basket.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="itemId">Item id.</param>
        /// <returns>Returns status code or <see cref="ValidationException"/>.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFromBasketAsync(string userId, int itemId)
        {
            try
            {
                await _basketService.DeleteAsync(userId, itemId);
                await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Available });
                return Ok();
            }
            catch (ValidationException e)
            {
                var validationExceptionSerialized = JsonConvert.SerializeObject(e);
                return BadRequest(validationExceptionSerialized);
            }
        }

        /// <summary>
        /// Delete all items by user id.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Returns status code.</returns>
        [HttpDelete("delete-all-by-user-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAllByUserIdAsync(string id)
        {
            await _basketService.DeleteAllByUserIdAsync(id);
            return Ok();
        }
    }
}
