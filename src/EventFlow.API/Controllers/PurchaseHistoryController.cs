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

        ////public async Task<IActionResult> Index()
        ////{
        ////    try
        ////    {
        ////        var user = _identityParser.Parse(HttpContext.User);
        ////        var vm = await _basketService.GetAllByUserAsync(user);
        ////        return View(vm);
        ////    }
        ////    catch (ArgumentException ex)
        ////    {
        ////        ModelState.AddModelError("", ex.Message);
        ////        return View();
        ////    }
        ////}

        ////[HttpPost]
        ////public async Task<IActionResult> Index(BasketViewModel model)
        ////{
        ////    try
        ////    {
        ////        var user = _identityParser.Parse(HttpContext.User);
        ////        var vm = await _basketService.GetAllByUserAsync(user);
        ////        var currentBalance = await _applicationUserService.GetBalanceAsync(user);
        ////        var totalPrice = vm.TotalPrice;

        ////        if (currentBalance >= totalPrice)
        ////        {
        ////            user.Balance = currentBalance - totalPrice;
        ////            await _applicationUserService.UpdateBalanceAsync(user);
        ////            var basketItems = _basketService.GetAll().Where(x => x.UserId == user.Id);
        ////            await _purchaseHistoryService.AddFromBasketAsync(basketItems);
        ////            foreach (var item in basketItems.ToList())
        ////            {
        ////                await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = item.ProductId, State = States.Purchased });
        ////            }

        ////            await _basketService.DeleteAsync(user);

        ////            return RedirectToAction("Index");
        ////        }
        ////        else
        ////        {
        ////            ViewData["NotEnoughMoney"] = _localizer["NotEnoughMoney"];
        ////        }

        ////        return View(vm);
        ////    }
        ////    catch (ArgumentException ex)
        ////    {
        ////        ModelState.AddModelError("", ex.Message);
        ////        return View();
        ////    }
        ////    catch (ValidationException ve)
        ////    {
        ////        ModelState.AddModelError("", ve.Message);
        ////        return View();
        ////    }
        ////}
    }
}
