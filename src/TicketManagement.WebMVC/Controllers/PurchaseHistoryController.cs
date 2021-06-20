using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize]
    public class PurchaseHistoryController : Controller
    {
        private readonly IPurchaseHistoryClient _purchaseHistoryClient;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;

        public PurchaseHistoryController(IPurchaseHistoryClient purchaseHistoryClient,
            IIdentityParser<ApplicationUser> identityParser, IMapper mapper)
        {
            _purchaseHistoryClient = purchaseHistoryClient;
            _identityParser = identityParser;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _identityParser.Parse(HttpContext.User).Id.ToString();
            var purchaseHistoryModel = await _purchaseHistoryClient.GetAllByUserIdAsync(userId);
            var vm = _mapper.Map<PurchaseHistoryModel, PurchaseHistoryViewModel>(purchaseHistoryModel);

            return View(vm);
        }
    }
}
