using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
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
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);

        public HomeController(IThirdPartyEventRepository thirdPartyEventRepository, IJsonSerializerService<ThirdPartyEvent> jsonSerializer)
        {
            _thirdPartyEventRepository = thirdPartyEventRepository;
            _jsonSerializer = jsonSerializer;
        }

        [ActionExecutionTime]
        public ActionResult Index()
        {
            var thirdPartyEvents = _thirdPartyEventRepository.GetAll().ToList();
            return View(thirdPartyEvents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ThirdPartyEvent thirdPartyEvent, HttpPostedFileBase posterImage)
        {
            if (posterImage != null)
            {
                thirdPartyEvent.PosterImage = FileUtil.ConvertImageToBase64(posterImage);
            }

            if (ModelState.IsValid)
            {
                _thirdPartyEventRepository.Create(thirdPartyEvent);
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
                _thirdPartyEventRepository.Update(thirdPartyEvent);
                return RedirectToAction("Index");
            }

            return View(thirdPartyEvent);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _thirdPartyEventRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewEventsJsonDetails()
        {
            byte[] filedata = System.IO.File.ReadAllBytes(_filePath);
            string contentType = MimeMapping.GetMimeMapping(_filePath);

            return File(filedata, contentType);
        }

        [HttpGet]
        public ActionResult DownloadEventsJsonDetails()
        {
            string fileName = Path.GetFileName(_filePath);
            byte[] filedata = System.IO.File.ReadAllBytes(_filePath);
            string contentType = MimeMapping.GetMimeMapping(_filePath);

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
            var thirdPartyEvent = _thirdPartyEventRepository.GetById(id);
            var thirdPartyEventJsonFormat = _jsonSerializer.SerializeObjectToJsonString(thirdPartyEvent);
            byte[] stringData = Encoding.UTF8.GetBytes(thirdPartyEventJsonFormat);
            string contentType = MimeMapping.GetMimeMapping(_filePath);

            return File(stringData, contentType);
        }

        [HttpGet]
        public ActionResult DownloadEventJsonDetails(int id)
        {
            var thirdPartyEvent = _thirdPartyEventRepository.GetById(id);
            var thirdPartyEventJsonFormat = _jsonSerializer.SerializeObjectToJsonString(thirdPartyEvent);

            byte[] stringData = Encoding.UTF8.GetBytes(thirdPartyEventJsonFormat);
            string contentType = MimeMapping.GetMimeMapping(_filePath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(stringData, contentType);
        }
    }
}