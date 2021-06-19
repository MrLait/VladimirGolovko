using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services.User.API.Models;
using TicketManagement.Services.User.API.Services;

namespace TicketManagement.Services.User.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<ApplicationUser> applicationUser,
            IIdentityParser<ApplicationUser> identityParser,
            IMapper mapper)
        {
            _applicationUserManager = applicationUser;
            _identityParser = identityParser;
            _mapper = mapper;
        }

        [HttpGet("index")]
        public async Task<IActionResult> IndexAsync()
        {
            var userId = _identityParser.Parse(HttpContext.User).Id;
            var user = await _applicationUserManager.FindByIdAsync(userId);
            var vm = _mapper.Map<ApplicationUser, ProfileViewModel>(user);
            return Ok(vm);
        }

        [HttpGet("editFirstName")]
        public async Task<IActionResult> EditFirstName()
        {
            var userId = _identityParser.Parse(HttpContext.User).Id;
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, FirstName = user.FirstName };
            return Ok(model);
        }

        [HttpPost("editFirstName")]
        public async Task<IActionResult> EditFirstName([FromForm] ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _applicationUserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;

                    var result = await _applicationUserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("editSurname")]
        public async Task<IActionResult> EditSurname()
        {
            var id = _identityParser.Parse(HttpContext.User).Id;
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, Surname = user.Surname };
            return Ok(model);
        }

        [HttpPost("editSurname")]
        public async Task<IActionResult> EditSurname([FromForm] ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _applicationUserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Surname = model.Surname;

                    var result = await _applicationUserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok(model);
        }

        [HttpGet("editEmail")]
        public async Task<IActionResult> EditEmail()
        {
            var id = _identityParser.Parse(HttpContext.User).Id;
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel { Id = user.Id, Email = user.Email };
            return Ok(model);
        }

        [HttpPost("editEmail")]
        public async Task<IActionResult> EditEmail([FromForm] ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _applicationUserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;

                    var result = await _applicationUserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok(model);
        }

        [HttpGet("editPassword")]
        public async Task<IActionResult> EditPassword()
        {
            var id = _identityParser.Parse(HttpContext.User).Id;
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ProfileChangePasswordViewModel model = new ProfileChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return Ok(model);
        }

        [HttpPost("editPassword")]
        public async Task<IActionResult> EditPassword([FromForm] ProfileChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _applicationUserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _applicationUserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                ModelState.AddModelError(string.Empty, "User is not found");
            }

            return Ok(model);
        }

        [HttpPost("editTimeZoneOffset")]
        public async Task<IActionResult> EditTimeZoneOffset(string timeZoneOffset, string returnUrl)
        {
            var id = _identityParser.Parse(HttpContext.User).Id;
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.TimeZoneOffset = timeZoneOffset;

                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return LocalRedirect(returnUrl);
        }

        [HttpPost("deposite")]
        public async Task<IActionResult> Deposite(decimal balance)
        {
            var id = _identityParser.Parse(HttpContext.User).Id;
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Balance += balance;

                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("setLanguage")]
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
