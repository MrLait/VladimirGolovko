using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TicketManagement.DataAccess.Domain.Enums;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.Basket;
using TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory;
using TicketManagement.WebMVC.Clients.IdentityClient.Profile;
using TicketManagement.WebMVC.Constants;
using TicketManagement.WebMVC.Infrastructure.ExceptionsMessages;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Basket controller.
    /// </summary>
    public class BasketController : Controller
    {
        private readonly IBasketClient _basketClient;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;
        private readonly IProfileClient _profileClient;
        private readonly IPurchaseHistoryClient _purchaseHistoryClient;
        private readonly IStringLocalizer<BasketController> _localizer;
        private readonly ILogger<BasketController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketController"/> class.
        /// </summary>
        /// <param name="basketClient">Basket client.</param>
        /// <param name="identityParser">Identity parser.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="profileClient">Profile client.</param>
        /// <param name="purchaseHistoryClient">Purchase history client.</param>
        /// <param name="localizer">Localizer.</param>
        /// <param name="logger">Logger.</param>
        public BasketController(IBasketClient basketClient,
            IIdentityParser<ApplicationUser> identityParser,
            IMapper mapper,
            IProfileClient profileClient,
            IPurchaseHistoryClient purchaseHistoryClient,
            IStringLocalizer<BasketController> localizer,
            ILogger<BasketController> logger)
        {
            _purchaseHistoryClient = purchaseHistoryClient;
            _profileClient = profileClient;
            _basketClient = basketClient;
            _identityParser = identityParser;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        /// <summary>
        /// Get index action.
        /// </summary>
        public async Task<IActionResult> IndexAsync()
        {
            var user = _identityParser.Parse(HttpContext.User);
            var basketModel = await _basketClient.GetAllByUserIdAsync(user.Id);
            var vm = _mapper.Map<BasketModel, BasketViewModel>(basketModel);

            return View(vm);
        }

        /// <summary>
        /// Post index action.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> IndexAsync(BasketViewModel model)
        {
            try
            {
                var userId = _identityParser.Parse(HttpContext.User).Id;
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

                    return RedirectToAction(BasketConst.Index);
                }

                ViewData[LocalizerConst.NotEnoughMoney] = _localizer[LocalizerConst.NotEnoughMoney];

                return View(vm);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ex);
                return View();
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ve);
                return View();
            }
        }

        /// <summary>
        /// Add to basket action.
        /// </summary>
        public async Task<IActionResult> AddToBasketAsync(EventAreaDto eventAreaDto, int itemId)
        {
            try
            {
                var userId = _identityParser.Parse(HttpContext.User).Id;
                await _basketClient.AddToBasketAsync(userId, itemId);

                return RedirectToAction(EventAreaConst.Index, EventAreaConst.ControllerName, new EventDto { Id = eventAreaDto.Id });
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ve);
                return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
            }
        }

        /// <summary>
        /// Remove from basket action.
        /// </summary>
        public async Task<IActionResult> RemoveFromBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        {
            if (itemState == States.Purchased)
            {
                return RedirectToAction(EventAreaConst.Index, EventAreaConst.ControllerName, new EventDto { Id = eventAreaDto.Id });
            }

            try
            {
                var userId = _identityParser.Parse(HttpContext.User).Id;
                await _basketClient.RemoveFromBasketAsync(userId, itemId);

                return RedirectToAction(EventAreaConst.Index, EventAreaConst.ControllerName, new EventDto { Id = eventAreaDto.Id });
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ve);
                return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
            }
        }
    }
}
