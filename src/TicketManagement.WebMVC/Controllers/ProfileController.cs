using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.ProfileViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;

        public ProfileController(UserManager<ApplicationUser> applicationUser)
        {
            _applicationUserManager = applicationUser;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user = await ParseAsync(HttpContext.User);

            var vm = new ProfileViewModel
            {
                Id = user.Id,
                Balance = user.Balance,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                Language = user.Language,
                TimeZone = user.TimeZoneOffset,
            };

            return View(vm);
        }

        public async Task<IActionResult> EditFirstName(string id)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
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
            }

            return View(model);
        }

        public async Task<IActionResult> EditSurname(string id)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
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

            return View(model);
        }

        public async Task<IActionResult> EditEmail(string id)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
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

            return View(model);
        }

        public async Task<IActionResult> EditPassword(string id)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> EditLanguage(string id, string language)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Language = language;

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

            return View();
        }

        public async Task<IActionResult> EditTimeZoneOffset(string id, string timeZoneOffset)
        {
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

            return View();
        }

        public async Task<IActionResult> Deposite(string id, decimal balance)
        {
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

            return View();
        }

        public async Task<ApplicationUser> ParseAsync(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                var id = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                ApplicationUser user = await _applicationUserManager.FindByIdAsync(id);
                return user;
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
