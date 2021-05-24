using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ClassicMvc.Models;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IThirdPartyEventRepository _thirdPartyEventRepository;

        public HomeController(IThirdPartyEventRepository thirdPartyEventRepository)
        {
            _thirdPartyEventRepository = thirdPartyEventRepository;
        }

        public ActionResult Index()
        {
            var thirdPartyEvents = _thirdPartyEventRepository.GetAll().ToList();
            ////            var circusEvent = new ThirdPartyEvent
            ////            {
            ////                Id = 1,
            ////                Name = "Почти серьезно",
            ////                EndDate = new DateTime(2021, 06, 30, 21, 00, 00),
            ////                StartDate = new DateTime(2021, 05, 30, 15, 00, 00),
            ////                PosterImage = await UploadSampleImage(),
            ////                Description = @"С 15 мая по 1 августа Белгосцирк и Московский цирк Ю.Никулина на
            ////Цветном бульваре представляют новую цирковую программу «Почти серьезно», посвященную 100-летию со Дня рождения Юрия Никулина!
            ////В программе- дрессированные лошади, медведи, козы, бразильское колесо смелости,мото-шар,
            ////эквилибристы на канате, акробаты на мачте, воздушные гимнасты, жонглеры и клоуны! Спешите!",
            ////            };
            return View(thirdPartyEvents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ThirdPartyEvent thirdPartyEvent)
        {
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
    }
}