using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventArea;
using TicketManagement.WebMVC.Constants;
using TicketManagement.WebMVC.ViewModels.EventAreaViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Event area controller.
    /// </summary>
    public class EventAreaController : Controller
    {
        private readonly IEventAreaClient _evenAreaClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaController"/> class.
        /// </summary>
        /// <param name="evenAreaClient">Even area client.</param>
        public EventAreaController(IEventAreaClient evenAreaClient)
        {
            _evenAreaClient = evenAreaClient;
        }

        /// <summary>
        /// Get index action.
        /// </summary>
        public async Task<IActionResult> Index(EventDto dto)
        {
            var eventArea = await _evenAreaClient.GetAllByEventIdAsync(dto.Id);
            var vm = new IndexViewModel { EventAreasItems = eventArea };
            return View(vm);
        }

        /// <summary>
        /// Get View event area action.
        /// </summary>
        public IActionResult GetViewEventArea(EventDto dto)
        {
            if (dto is null)
            {
                return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
            }

            if (dto.Id != 0)
            {
                return RedirectToAction(EventAreaConst.Index, EventAreaConst.ControllerName, dto);
            }

            return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
        }
    }
}