using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClassicMvc.Infrastructure.Filters;
using ClassicMvc.Infrastructure.Loggers;
using ClassicMvc.Models;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IThirdPartyEventRepository _thirdPartyEventRepository;
        private readonly IJsonSerializer<ThirdPartyEvent> _jsonSerializer;

        public HomeController(IThirdPartyEventRepository thirdPartyEventRepository, IJsonSerializer<ThirdPartyEvent> jsonSerializer)
        {
            _thirdPartyEventRepository = thirdPartyEventRepository;
            _jsonSerializer = jsonSerializer;
        }

        [ActionExecutionTime]
        public async Task<ActionResult> Index()
        {
            var thirdPartyEvents = (await _thirdPartyEventRepository.GetAllAsync()).ToList();
            return View(thirdPartyEvents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ThirdPartyEvent thirdPartyEvent)
        {
            if (ModelState.IsValid)
            {
                await _thirdPartyEventRepository.CreateAsync(thirdPartyEvent);
                return RedirectToAction("Index");
            }

            return View(thirdPartyEvent);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var thirdPartyEvent = _thirdPartyEventRepository.GetById(id);
            return View(thirdPartyEvent);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ThirdPartyEvent thirdPartyEvent)
        {
            if (ModelState.IsValid)
            {
                await _thirdPartyEventRepository.UpdateAsync(thirdPartyEvent);
                return RedirectToAction("Index");
            }

            return View(thirdPartyEvent);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            await _thirdPartyEventRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetJsonContent()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);
            string fileName = Path.GetFileName(filePath);
            byte[] filedata = System.IO.File.ReadAllBytes(filePath);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);

            var thirdPartyEvent = _thirdPartyEventRepository.GetById(id);
            var thirdPartyEventJsonFormat = _jsonSerializer.SerializeObjectToJsonString(thirdPartyEvent);

            byte[] stringData = Encoding.UTF8.GetBytes(thirdPartyEventJsonFormat);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            Response.AppendHeader("Content-Disposition", contentType);
            return File(stringData, contentType);
        }
    }
}