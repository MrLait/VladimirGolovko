using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClassicMvc.Infrastructure.Filters;
using ClassicMvc.Infrastructure.Utils;
using ClassicMvc.Services;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IThirdPartyEventRepository _thirdPartyEventRepository;
        private readonly IJsonSerializerService<ThirdPartyEvent> _jsonSerializer;

        public HomeController(IThirdPartyEventRepository thirdPartyEventRepository, IJsonSerializerService<ThirdPartyEvent> jsonSerializer)
        {
            _thirdPartyEventRepository = thirdPartyEventRepository;
            _jsonSerializer = jsonSerializer;
        }

        public ActionResult Index()
        {
            var thirdPartyEvents = _thirdPartyEventRepository.GetAllAsync().ToList();
            return View(thirdPartyEvents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [ActionExecutionTime]
        [HttpPost]
        public ActionResult Create(ThirdPartyEvent thirdPartyEvent, HttpPostedFileBase file)
        {
            if (file != null)
            {
                thirdPartyEvent.PosterImage = FileUtil.ConvertImageToBase64(file);
            }

            if (ModelState.IsValid)
            {
                _thirdPartyEventRepository.CreateAsync(thirdPartyEvent);
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
        public ActionResult Edit(ThirdPartyEvent thirdPartyEvent)
        {
            if (ModelState.IsValid)
            {
                _thirdPartyEventRepository.UpdateAsync(thirdPartyEvent);
                return RedirectToAction("Index");
            }

            return View(thirdPartyEvent);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _thirdPartyEventRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewEventsJsonDetails()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);
            byte[] filedata = System.IO.File.ReadAllBytes(filePath);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            return File(filedata, contentType);
        }

        [HttpGet]
        public ActionResult DownloadEventsJsonDetails()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);
            string fileName = Path.GetFileName(filePath);
            byte[] filedata = System.IO.File.ReadAllBytes(filePath);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        [HttpGet]
        public ActionResult ViewEventJsonDetails(int id)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);
            var thirdPartyEvent = _thirdPartyEventRepository.GetById(id);
            var thirdPartyEventJsonFormat = _jsonSerializer.SerializeObjectToJsonString(thirdPartyEvent);
            byte[] stringData = Encoding.UTF8.GetBytes(thirdPartyEventJsonFormat);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            return File(stringData, contentType);
        }

        [HttpGet]
        public ActionResult DownloadEventJsonDetails(int id)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);

            var thirdPartyEvent = _thirdPartyEventRepository.GetById(id);
            var thirdPartyEventJsonFormat = _jsonSerializer.SerializeObjectToJsonString(thirdPartyEvent);

            byte[] stringData = Encoding.UTF8.GetBytes(thirdPartyEventJsonFormat);
            string contentType = MimeMapping.GetMimeMapping(filePath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(stringData, contentType);
        }
    }
}