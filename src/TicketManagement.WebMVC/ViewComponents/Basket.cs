using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;

namespace TicketManagement.WebMVC.ViewComponents
{
    public class Basket : ViewComponent
    {
        private readonly IBasketService _basketService;

        public Basket(IBasketService basketService) => _basketService = basketService;

        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new BasketComponentViewModel();
            try
            {
                var itemsInCart = await ItemsInCartAsync(user);
                vm.ItemsCount = itemsInCart;
                return View(vm);
            }
            catch
            {
                ViewBag.IsBasketInoperative = true;
            }

            return View(vm);
        }

        private async Task<int> ItemsInCartAsync(ApplicationUser user)
        {
            var basket = await _basketService.GetAllByUserAsync(user);
            return basket.Items.Count;
        }
    }
}
