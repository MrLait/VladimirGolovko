using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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

        public BasketController(IBasketService basketService, IApplicationUserService applicationUserService, IPurchaseHistoryService purchaseHistoryService, IEventSeatService eventSeatService)
        {
            _basketService = basketService;
            _applicationUserService = applicationUserService;
            _purchaseHistoryService = purchaseHistoryService;
            _eventSeatService = eventSeatService;
        }

        public async Task<IActionResult> Index()
        {
            var user = Parse(HttpContext.User);
            var vm = await _basketService.GetAllByUserAsync(user);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BasketViewModel model)
        {
            var user = Parse(HttpContext.User);
            var vm = await _basketService.GetAllByUserAsync(user);
            var currentBalance = await _applicationUserService.GetBalanceAsync(user);
            var totalPrice = vm.TotalPrice;

            if (currentBalance >= totalPrice)
            {
                user.Balance = currentBalance - totalPrice;
                await _applicationUserService.UpdateBalanceAsync(user);
                var basketItems = (await _basketService.GetAllAsync()).Where(x => x.UserId == user.Id);
                await _purchaseHistoryService.AddFromBasketAsync(basketItems);
                foreach (var item in basketItems)
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

        public ApplicationUser Parse(IPrincipal principal)
        {
            // Pattern matching 'is' expression
            // assigns "claims" if "principal" is a "ClaimsPrincipal"
            if (principal is ClaimsPrincipal claims)
            {
                return new ApplicationUser
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "",
                    PhoneNumber = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value ?? "",
                    UserName = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "",
                };
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
