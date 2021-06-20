using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Clients.IdentityClient.Profile;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.ProfileViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileClient _profileClient;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;

        public ProfileController(IProfileClient profileClient,
            IIdentityParser<ApplicationUser> identityParser,
            IMapper mapper)
        {
            _profileClient = profileClient;
            _identityParser = identityParser;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var userId = _identityParser.Parse(HttpContext.User).Id;
            var user = await _profileClient.GetUserProfile(userId);
            var vm = _mapper.Map<ApplicationUser, ProfileViewModel>(user);

            return View(vm);
        }

        public async Task<IActionResult> EditFirstName(string id)
        {
            ApplicationUser user = await _profileClient.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, FirstName = user.FirstName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditFirstName(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _profileClient.GetUserProfile(model.Id);
                if (user != null)
                {
                    await _profileClient.EditFirstName(model.Id, model.FirstName);
                    return RedirectToAction("Index");
                }

                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> EditSurname(string id)
        {
            ApplicationUser user = await _profileClient.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, Surname = user.Surname };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSurname(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _profileClient.GetUserProfile(model.Id);
                if (user != null)
                {
                    await _profileClient.EditSurname(user.Id, model.Surname);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> EditEmail(string id)
        {
            ApplicationUser user = await _profileClient.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmail(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _profileClient.GetUserProfile(model.Id);
                if (user != null)
                {
                    await _profileClient.EditEmail(model.Id, model.Email);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> EditPassword(string id)
        {
            ApplicationUser user = await _profileClient.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileChangePasswordViewModel model = new ProfileChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(ProfileChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _profileClient.GetUserProfile(model.Id);
                if (user != null)
                {
                    await _profileClient.EditPassword(user.Id.ToString(), model.OldPassword, model.NewPassword);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "User is not found");
            }

            return View(model);
        }

        public async Task<IActionResult> EditTimeZoneOffset(string id, string timeZoneOffset, string returnUrl)
        {
            ApplicationUser user = await _profileClient.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            await _profileClient.EditTimeZoneOffset(id, timeZoneOffset);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Deposite(string id, decimal balance, string returnUrl)
        {
            ApplicationUser user = await _profileClient.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            await _profileClient.Deposite(id, balance);

            return RedirectToAction("Index");
        }

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
