using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TicketManagement.WebMVC.Clients.IdentityClient.AccountUser;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserClient _applicationUserClient;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserClient applicationUserClient,
            IMapper mapper,
            IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger)
        {
            _mapper = mapper;
            _applicationUserClient = applicationUserClient;
            _localizer = localizer;
            _logger = logger;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var user = _mapper.Map<RegisterViewModel, RegisterModel>(vm);
                var token = await _applicationUserClient.Register(user);

                HttpContext.Response.Cookies.Append("secret_jwt_key", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                return RedirectToAction("Index", "EventHomePage");
            }
            catch (HttpRequestException e)
            {
                var erorrMessages = e.Message.Split(',');
                foreach (var error in erorrMessages)
                {
                    switch (error)
                    {
                        case "PasswordTooShort":
                            ModelState.AddModelError(string.Empty, _localizer["PasswordTooShort"]);
                            continue;
                        case "PasswordRequiresNonAlphanumeric":
                            ModelState.AddModelError(string.Empty, _localizer["PasswordRequiresNonAlphanumeric"]);
                            continue;
                        case "PasswordRequiresDigit":
                            ModelState.AddModelError(string.Empty, _localizer["PasswordRequiresDigit"]);
                            continue;
                        case "PasswordRequiresUpper":
                            ModelState.AddModelError(string.Empty, _localizer["PasswordRequiresUpper"]);
                            continue;
                        default:
                            break;
                    }
                }

                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, e);
            }

            return View(vm);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var user = _mapper.Map<LoginViewModel, LoginModel>(vm);
                var token = await _applicationUserClient.Login(user);
                HttpContext.Response.Cookies.Append("secret_jwt_key", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                return RedirectToAction("Index", "EventHomePage");
            }
            catch (HttpRequestException e)
            {
                var result = JsonConvert.DeserializeObject<Microsoft.AspNetCore.Identity.SignInResult>(e.Message);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, _localizer["Incorrect username and(or) password"]);
                }

                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, e);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Append("secret_jwt_key", "");
            return RedirectToAction("Index", "EventHomePage");
        }
    }
}
