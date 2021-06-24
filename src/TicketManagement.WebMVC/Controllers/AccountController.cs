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
using TicketManagement.WebMVC.Constants;
using TicketManagement.WebMVC.JwtTokenAuth;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserClient _applicationUserClient;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="applicationUserClient">Application user client.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="localizer">Localizer.</param>
        /// <param name="logger">Logger.</param>
        public AccountController(
            IUserClient applicationUserClient,
            IMapper mapper,
            IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger)
        {
            _mapper = mapper;
            _applicationUserClient = applicationUserClient;
            _localizer = localizer;
            _logger = logger;
        }

        /// <summary>
        /// Get register action.
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Post register action.
        /// </summary>
        [HttpPost]
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

                HttpContext.Response.Cookies.Append(JwtAuthenticationConstants.SecretJwtKey, token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
            }
            catch (HttpRequestException e)
            {
                var errorMessages = e.Message.Split(',');
                foreach (var error in errorMessages)
                {
                    switch (error)
                    {
                        case IdentityErrorConst.PasswordTooShort:
                            ModelState.AddModelError(string.Empty, _localizer[IdentityErrorConst.PasswordTooShort]);
                            continue;
                        case IdentityErrorConst.PasswordRequiresNonAlphanumeric:
                            ModelState.AddModelError(string.Empty, _localizer[IdentityErrorConst.PasswordRequiresNonAlphanumeric]);
                            continue;
                        case IdentityErrorConst.PasswordRequiresDigit:
                            ModelState.AddModelError(string.Empty, _localizer[IdentityErrorConst.PasswordRequiresDigit]);
                            continue;
                        case IdentityErrorConst.PasswordRequiresUpper:
                            ModelState.AddModelError(string.Empty, _localizer[IdentityErrorConst.PasswordRequiresUpper]);
                            continue;
                        default:
                            break;
                    }
                }

                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, e);
            }

            return View(vm);
        }

        /// <summary>
        /// Get login action.
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Post login action.
        /// </summary>
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
                HttpContext.Response.Cookies.Append(JwtAuthenticationConstants.SecretJwtKey, token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
            }
            catch (HttpRequestException e)
            {
                var result = JsonConvert.DeserializeObject<Microsoft.AspNetCore.Identity.SignInResult>(e.Message);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, _localizer[IdentityErrorConst.IncorrectUsernameAndOrPassword]);
                }

                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, e);
            }

            return View(vm);
        }

        /// <summary>
        /// Post logout action.
        /// </summary>
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Append(JwtAuthenticationConstants.SecretJwtKey, "");
            return RedirectToAction(EventHomePageConst.Index, EventHomePageConst.ControllerName);
        }
    }
}
