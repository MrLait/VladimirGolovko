using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Enums;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IEventSeatService _eventSeatService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IStringLocalizer<BasketController> _localizer;

        public BasketController(IBasketService basketService,
            IApplicationUserService applicationUserService,
            IPurchaseHistoryService purchaseHistoryService,
            IEventSeatService eventSeatService,
            IIdentityParser<ApplicationUser> identityParser,
            IStringLocalizer<BasketController> localizer)
        {
            _basketService = basketService;
            _applicationUserService = applicationUserService;
            _purchaseHistoryService = purchaseHistoryService;
            _eventSeatService = eventSeatService;
            _identityParser = identityParser;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var user = _identityParser.Parse(HttpContext.User);
                var vm = await _basketService.GetAllByUserAsync(user);
                return View(vm);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(BasketViewModel model)
        {
            try
            {
                var user = _identityParser.Parse(HttpContext.User);
                var vm = await _basketService.GetAllByUserAsync(user);
                var currentBalance = await _applicationUserService.GetBalanceAsync(user);
                var totalPrice = vm.TotalPrice;

                if (currentBalance >= totalPrice)
                {
                    user.Balance = currentBalance - totalPrice;
                    await _applicationUserService.UpdateBalanceAsync(user);
                    var basketItems = _basketService.GetAll().Where(x => x.UserId == user.Id);
                    await _purchaseHistoryService.AddFromBasketAsync(basketItems);
                    foreach (var item in basketItems.ToList())
                    {
                        await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = item.ProductId, State = States.Purchased });
                    }

                    await _basketService.DeleteAsync(user);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["NotEnoughMoney"] = _localizer["NotEnoughMoney"];
                }

                return View(vm);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                return View();
            }
        }
    }
}
