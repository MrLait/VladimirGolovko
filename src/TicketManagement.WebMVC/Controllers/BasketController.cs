using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.DataAccess.Enums;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.Basket;
using TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory;
using TicketManagement.WebMVC.Clients.IdentityClient.Profile;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketClient _basketClient;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;
        private readonly IProfileClient _profileClient;
        private readonly IPurchaseHistoryClient _purchaseHistoryClient;
        private readonly IStringLocalizer<BasketController> _localizer;

        ////private readonly IBasketService _basketService;
        ////private readonly IApplicationUserService _applicationUserService;
        ////private readonly IPurchaseHistoryService _purchaseHistoryService;
        ////private readonly IEventSeatService _eventSeatService;

        public BasketController(IBasketClient basketClient,
            IIdentityParser<ApplicationUser> identityParser,
            IMapper mapper,
            IProfileClient profileClient,
            IPurchaseHistoryClient purchaseHistoryClient,
            IStringLocalizer<BasketController> localizer)
            ////IBasketService basketService,
            ////IApplicationUserService applicationUserService,
            ////IPurchaseHistoryService purchaseHistoryService,
            ////IEventSeatService eventSeatService,
            ////IIdentityParser<ApplicationUser> identityParser,
        {
            _purchaseHistoryClient = purchaseHistoryClient;
            _profileClient = profileClient;
            _basketClient = basketClient;
            _identityParser = identityParser;
            _mapper = mapper;
            _localizer = localizer;
            ////_basketService = basketService;
            ////_applicationUserService = applicationUserService;
            ////_purchaseHistoryService = purchaseHistoryService;
            ////_eventSeatService = eventSeatService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user = _identityParser.Parse(HttpContext.User);
            var basketModel = await _basketClient.GetAllByUserIdAsync(user.Id);
            var vm = _mapper.Map<BasketModel, BasketViewModel>(basketModel);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(BasketViewModel model)
        {
            try
            {
                var userId = _identityParser.Parse(HttpContext.User).Id.ToString();
                var basketModel = await _basketClient.GetAllByUserIdAsync(userId);
                var vm = _mapper.Map<BasketModel, BasketViewModel>(basketModel);

                var currentBalance = await _profileClient.GetBalanceAsync(userId);
                var totalPrice = vm.TotalPrice;

                if (currentBalance >= totalPrice)
                {
                    var newBalance = currentBalance - totalPrice;
                    await _profileClient.UpdateBalanceAsync(userId, newBalance);

                    foreach (var item in basketModel.Items)
                    {
                        await _purchaseHistoryClient.AddItemAsync(userId, item.Id);
                    }

                    await _basketClient.DeleteAllByUserIdAsync(userId);

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

        public async Task<IActionResult> AddToBasketAsync(EventAreaDto eventAreaDto, int itemId)
        {
            try
            {
                var userId = _identityParser.Parse(HttpContext.User).Id.ToString();
                await _basketClient.AddToBasketAsync(userId, itemId);

                return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                return RedirectToAction("Index", "EventHomePage");
            }
        }

        public async Task<IActionResult> RemoveFromBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        {
            if (itemState == States.Purchased)
            {
                return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
            }

            try
            {
                var userId = _identityParser.Parse(HttpContext.User).Id.ToString();
                await _basketClient.RemoveFromBasketAsync(userId, itemId);

                return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                return RedirectToAction("Index", "EventHomePage");
            }
        }
    }
}
