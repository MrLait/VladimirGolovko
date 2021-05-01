using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    ////[Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        ////[Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
        public IActionResult SignIn(string returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                ////return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }

            ////return RedirectToAction("Index", "Home");

            ////var token = await HttpContext.GetTokenAsync("access_token");

            ////if (token != null)
            ////{
            ////    ViewData["access_token"] = token;
            ////}

            // "Catalog" because UrlHelper doesn't support nameof() for controllers
            //// https://github.com/aspnet/Mvc/issues/5853
            return RedirectToAction(nameof(EventHomePageController.Index), "EventHomePage");
        }

        /// <summary>
        /// Show login page.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LoginAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        /////// <summary>
        /////// Handle postback from username/password login
        /////// </summary>
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            ////if (ModelState.IsValid)
            ////{
            ////    var user = await _loginService.FindByUsername(model.Email);

            ////    if (await _loginService.ValidateCredentials(user, model.Password))
            ////    {
            ////        var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);

            ////        var props = new AuthenticationProperties
            ////        {
            ////            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
            ////            AllowRefresh = true,
            ////            RedirectUri = model.ReturnUrl
            ////        };

            ////        if (model.RememberMe)
            ////        {
            ////            var permanentTokenLifetime = _configuration.GetValue("PermanentTokenLifetimeDays", 365);

            ////            props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
            ////            props.IsPersistent = true;
            ////        };

            ////        await _loginService.SignInAsync(user, props);

            ////        // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
            ////        if (_interaction.IsValidReturnUrl(model.ReturnUrl))
            ////        {
            ////            return Redirect(model.ReturnUrl);
            ////        }

            ////        return Redirect("~/");
            ////    }

            ////    ModelState.AddModelError("", "Invalid username or password.");
            ////}

            ////// something went wrong, show form with error
            ////var vm = await BuildLoginViewModelAsync(model);

            ////ViewData["ReturnUrl"] = model.ReturnUrl;

            return View();
        }
    }
}
