////using System;
////using System.Linq;
////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.Extensions.Localization;
////using TicketManagement.BusinessLogic.Infrastructure;
////using TicketManagement.BusinessLogic.Interfaces;
////using TicketManagement.DataAccess.Enums;
////using TicketManagement.Dto;
////using TicketManagement.Services.Basket.API.Models;
////using TicketManagement.Services.Basket.API.Services;

////namespace TicketManagement.Services.Basket.API.Controllers
////{
////    [Authorize]
////    [Route("api/[controller]")]
////    [ApiController]
////    public class BasketController : ControllerBase
////    {
////        private readonly IBasketService _basketService;
////        private readonly IApplicationUserService _applicationUserService;
////        private readonly IPurchaseHistoryService _purchaseHistoryService;
////        private readonly IEventSeatService _eventSeatService;
////        private readonly IIdentityParser<ApplicationUser> _identityParser;

////        public BasketController(
////        IBasketService basketService,
////        IApplicationUserService applicationUserService,
////        IPurchaseHistoryService purchaseHistoryService,
////        IEventSeatService eventSeatService,
////        IIdentityParser<ApplicationUser> identityParser)
////        {
////            _basketService = basketService;
////            _applicationUserService = applicationUserService;
////            _purchaseHistoryService = purchaseHistoryService;
////            _eventSeatService = eventSeatService;
////            _identityParser = identityParser;
////        }

////        [HttpGet("index")]
////        public async Task<IActionResult> IndexAsync()
////        {
////            try
////            {
////                var user = _identityParser.Parse(HttpContext.User);
////                var vm = await _basketService.GetAllByUserAsync(user);
////                return Ok(vm);
////            }
////            catch (ArgumentException ex)
////            {
////                ModelState.AddModelError("", ex.Message);
////                return Ok();
////            }
////        }

////        [HttpPost("purchase")]
////        public async Task<IActionResult> Purchase()
////        {
////            try
////            {
////                var user = _identityParser.Parse(HttpContext.User);
////                var vm = await _basketService.GetAllByUserAsync(user);
////                var currentBalance = await _applicationUserService.GetBalanceAsync(user);
////                var totalPrice = vm.TotalPrice;

////                if (currentBalance >= totalPrice)
////                {
////                    user.Balance = currentBalance - totalPrice;
////                    await _applicationUserService.UpdateBalanceAsync(user);
////                    var basketItems = _basketService.GetAll().Where(x => x.UserId == user.Id);
////                    await _purchaseHistoryService.AddFromBasketAsync(basketItems);
////                    foreach (var item in basketItems.ToList())
////                    {
////                        await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = item.ProductId, State = States.Purchased });
////                    }

////                    await _basketService.DeleteAsync(user);

////                    return RedirectToAction("Index");
////                }
////                else
////                {
////                    /////ViewData["NotEnoughMoney"] = _localizer["NotEnoughMoney"];
////                }

////                return Ok(vm);
////            }
////            catch (ArgumentException ex)
////            {
////                ModelState.AddModelError("", ex.Message);
////                return Ok();
////            }
////            catch (ValidationException ve)
////            {
////                ModelState.AddModelError("", ve.Message);
////                return Ok();
////            }
////        }
////    }
////}
