﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;

namespace TicketManagement.WebMVC.Controllers
{
    public class PurchaseHistoryController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPurchaseHistoryService _purchaseHistoryService;

        public PurchaseHistoryController(IBasketService basketService, IApplicationUserService applicationUserService, IPurchaseHistoryService purchaseHistoryService)
        {
            _basketService = basketService;
            _applicationUserService = applicationUserService;
            _purchaseHistoryService = purchaseHistoryService;
        }

        public async Task<IActionResult> Index()
        {
            var user = Parse(HttpContext.User);
            var vm = await _purchaseHistoryService.GetAllByUserAsync(user);

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