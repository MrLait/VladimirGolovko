using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Clients.IdentityClient.Profile;
using TicketManagement.WebMVC.Constants;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.ProfileViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Profile controller.
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileClient _profileClient;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="profileClient">Profile client.</param>
        /// <param name="identityParser">Identity parser.</param>
        /// <param name="mapper">Mapper.</param>
        public ProfileController(IProfileClient profileClient,
            IIdentityParser<ApplicationUser> identityParser,
            IMapper mapper)
        {
            _profileClient = profileClient;
            _identityParser = identityParser;
            _mapper = mapper;
        }

        /// <summary>
        /// View index.
        /// </summary>
        public async Task<IActionResult> IndexAsync()
        {
            var userId = _identityParser.Parse(HttpContext.User).Id;
            var user = await _profileClient.GetUserProfileAsync(userId);
            var vm = _mapper.Map<ApplicationUser, ProfileViewModel>(user);

            return View(vm);
        }

        /// <summary>
        /// Edit first name.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditFirstName(string id)
        {
            var user = await _profileClient.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel { Id = user.Id, FirstName = user.FirstName };
            return View(model);
        }

        /// <summary>
        /// Edit first name.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditFirstName(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _profileClient.GetUserProfileAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            await _profileClient.EditFirstNameAsync(model.Id, model.FirstName);
            return RedirectToAction(ProfileConst.Index);
        }

        /// <summary>
        /// Edit surname.
        /// </summary>
        public async Task<IActionResult> EditSurname(string id)
        {
            var user = await _profileClient.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel { Id = user.Id, Surname = user.Surname };
            return View(model);
        }

        /// <summary>
        /// Edit surname.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditSurname(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _profileClient.GetUserProfileAsync(model.Id);
            if (user == null)
            {
                return View(model);
            }

            await _profileClient.EditSurnameAsync(user.Id, model.Surname);
            return RedirectToAction(ProfileConst.Index);
        }

        /// <summary>
        /// Edit email.
        /// </summary>
        public async Task<IActionResult> EditEmail(string id)
        {
            var user = await _profileClient.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        /// <summary>
        /// Edit email.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditEmail(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _profileClient.GetUserProfileAsync(model.Id);
            if (user == null)
            {
                return View(model);
            }

            await _profileClient.EditEmailAsync(model.Id, model.Email);
            return RedirectToAction(ProfileConst.Index);
        }

        /// <summary>
        /// Edit password.
        /// </summary>
        public async Task<IActionResult> EditPassword(string id)
        {
            var user = await _profileClient.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        /// <summary>
        /// Edit password.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditPassword(ProfileChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _profileClient.GetUserProfileAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, LocalizerConst.UserIsNotFound);
                return View(model);
            }

            await _profileClient.EditPasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            return RedirectToAction(ProfileConst.Index);
        }

        /// <summary>
        /// Edit time zone offset.
        /// </summary>
        public async Task<IActionResult> EditTimeZoneOffset(string id, string timeZoneOffset, string returnUrl)
        {
            var user = await _profileClient.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _profileClient.EditTimeZoneOffsetAsync(id, timeZoneOffset);
            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Deposit.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Deposit(string id, decimal balance, string returnUrl)
        {
            var user = await _profileClient.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _profileClient.DepositAsync(id, balance);
            return RedirectToAction(ProfileConst.Index);
        }

        /// <summary>
        /// Set language.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                });

            return LocalRedirect(returnUrl);
        }
    }
}
