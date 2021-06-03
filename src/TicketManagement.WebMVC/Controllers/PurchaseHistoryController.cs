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
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;

        public PurchaseHistoryController(IPurchaseHistoryService purchaseHistoryService,
            IIdentityParser<ApplicationUser> identityParser)
        {
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
