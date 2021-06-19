using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Clients.IdentityClient;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserClient _applicationUserClient;
        ////private readonly UserManager<ApplicationUser> _userManager;
        ////private readonly SignInManager<ApplicationUser> _signInManager;
        ////private readonly IMapper _mapper;
        ////private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(IUserClient applicationUserClient)
            ////UserManager<ApplicationUser> userManager,
            ////SignInManager<ApplicationUser> signInManager,
            ////IMapper mapper, IStringLocalizer<AccountController> localizer
        {
            ////_userManager = userManager;
            ////_signInManager = signInManager;
            ////_mapper = mapper;
            ////_localizer = localizer;
            _applicationUserClient = applicationUserClient;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel user)
        {
            var token = await _applicationUserClient.Register(user);
            HttpContext.Response.Cookies.Append("secret_jwt_key", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            });

            return Ok();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel user)
        {
            var token = await _applicationUserClient.Login(user);
            HttpContext.Response.Cookies.Append("secret_jwt_key", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            });

            return RedirectToAction("Index", "EventHomePage");
        }

        ////[HttpGet]
        ////public IActionResult Register()
        ////{
        ////    return View();
        ////}

        ////[HttpPost]
        ////public async Task<IActionResult> Register(RegisterViewModel model)
        ////{
        ////if (ModelState.IsValid)
        ////{
        ////    var user = _mapper.Map<RegisterViewModel, ApplicationUser>(model);
        ////    var result = await _userManager.CreateAsync(user, model.Password);
        ////    if (result.Succeeded)
        ////    {
        ////        await _userManager.AddToRoleAsync(user, UserRoles.User);
        ////        await _signInManager.SignInAsync(user, false);
        ////        await _userManager.AddClaimAsync(user, CreateClaim("TimeZoneOffset", user.TimeZoneOffset));
        ////        await _userManager.AddClaimAsync(user, CreateClaim("Language", user.Language));
        ////        return RedirectToAction("Index", "EventHomePage");
        ////    }

        ////    foreach (var error in result.Errors)
        ////    {
        ////        switch (error.Code)
        ////        {
        ////            case "PasswordTooShort":
        ////                ModelState.AddModelError(string.Empty, _localizer["PasswordTooShort"]);
        ////                continue;
        ////            case "PasswordRequiresNonAlphanumeric":
        ////                ModelState.AddModelError(string.Empty, _localizer["PasswordRequiresNonAlphanumeric"]);
        ////                continue;
        ////            case "PasswordRequiresDigit":
        ////                ModelState.AddModelError(string.Empty, _localizer["PasswordRequiresDigit"]);
        ////                continue;
        ////            case "PasswordRequiresUpper":
        ////                ModelState.AddModelError(string.Empty, _localizer["PasswordRequiresUpper"]);
        ////                continue;
        ////            default:
        ////                break;
        ////        }

        ////        ModelState.AddModelError(string.Empty, error.Description);
        ////    }
        ////}

        ////return View(model);
        ////}

        ////[HttpGet]
        ////public IActionResult Login(string returnUrl = null)
        ////{
        ////    return View(new LoginViewModel { ReturnUrl = returnUrl });
        ////}

        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Login(LoginViewModel model)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        var result =
        ////            await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        ////        if (result.Succeeded)
        ////        {
        ////            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        ////            {
        ////                return Redirect(model.ReturnUrl);
        ////            }

        ////            return RedirectToAction("Index", "EventHomePage");
        ////        }

        ////        ModelState.AddModelError(string.Empty, _localizer["Incorrect username and(or) password"]);
        ////    }

        ////    return View(model);
        ////}

        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Logout()
        ////{
        ////    await _signInManager.SignOutAsync();
        ////    return RedirectToAction("Index", "EventHomePage");
        ////}

        ////private static Claim CreateClaim(string type, string value)
        ////{
        ////    return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        ////}
    }
}
