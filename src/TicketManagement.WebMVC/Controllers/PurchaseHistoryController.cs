using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize]
    public class PurchaseHistoryController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;

        public PurchaseHistoryController(IBasketService basketService, IApplicationUserService applicationUserService, IPurchaseHistoryService purchaseHistoryService,
            IIdentityParser<ApplicationUser> identityParser)
        {
            _basketService = basketService;
            _applicationUserService = applicationUserService;
            _purchaseHistoryService = purchaseHistoryService;
            _identityParser = identityParser;
        }

        public async Task<IActionResult> Index()
        {
            var user = _identityParser.Parse(HttpContext.User);
            var vm = await _purchaseHistoryService.GetAllByUserAsync(user);

            return View(vm);
        }
    }
}
