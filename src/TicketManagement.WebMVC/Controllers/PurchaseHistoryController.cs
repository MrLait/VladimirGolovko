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
    /// <summary>
    /// Purchase history controller.
    /// </summary>
    [Authorize]
    public class PurchaseHistoryController : Controller
    {
        private readonly IPurchaseHistoryClient _purchaseHistoryClient;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseHistoryController"/> class.
        /// </summary>
        /// <param name="purchaseHistoryClient">Purchase history client.</param>
        /// <param name="identityParser">Identity parser.</param>
        /// <param name="mapper">Mapper.</param>
        public PurchaseHistoryController(IPurchaseHistoryClient purchaseHistoryClient,
            IIdentityParser<ApplicationUser> identityParser, IMapper mapper)
        {
            _purchaseHistoryClient = purchaseHistoryClient;
            _identityParser = identityParser;
            _mapper = mapper;
        }

        /// <summary>
        /// View index.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = _identityParser.Parse(HttpContext.User).Id;
            var purchaseHistoryModel = await _purchaseHistoryClient.GetAllByUserIdAsync(userId);
            var vm = _mapper.Map<PurchaseHistoryModel, PurchaseHistoryViewModel>(purchaseHistoryModel);

            return View(vm);
        }
    }
}
