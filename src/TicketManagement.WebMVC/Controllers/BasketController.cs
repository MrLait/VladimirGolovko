using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public BasketController(IBasketService basketService,
            IApplicationUserService applicationUserService,
            IPurchaseHistoryService purchaseHistoryService,
            IEventSeatService eventSeatService,
            IIdentityParser<ApplicationUser> identityParser)
        {
            _basketService = basketService;
            _applicationUserService = applicationUserService;
            _purchaseHistoryService = purchaseHistoryService;
            _eventSeatService = eventSeatService;
            _identityParser = identityParser;
        }

        public async Task<IActionResult> Index()
        {
            var user = _identityParser.Parse(HttpContext.User);
            var vm = await _basketService.GetAllByUserAsync(user);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BasketViewModel model)
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
                ModelState.AddModelError("", "Not enough money.");
            }

            return View(vm);
        }
    }
}
