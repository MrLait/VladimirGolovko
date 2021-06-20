using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;

        public ProfileController(SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            UserManager<ApplicationUser> applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        [HttpGet("getBalance")]
        public async Task<ActionResult> GetBalanceAsync(string userId)
        {
            return Ok((await _applicationUserManager.FindByIdAsync(userId)).Balance);
        }

        [HttpGet("updateBalance")]
        public async Task<ActionResult> UpdateBalanceAsync(string userId, decimal balance)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            user.Balance = balance;
            await _applicationUserManager.UpdateAsync(user);
            return Ok();
        }

        [HttpGet("getUserProfile")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("editFirstName")]
        public async Task<IActionResult> EditFirstName(string userId, string firstName)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.FirstName = firstName;
                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("editSurname")]
        public async Task<IActionResult> EditSurname(string userId, string surname)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Surname = surname;
                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("editEmail")]
        public async Task<IActionResult> EditEmail(string userId, string email)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Email = email;
                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("editPassword")]
        public async Task<IActionResult> EditPassword(string userId, string oldPassword, string newPassword)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _applicationUserManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("editTimeZoneOffset")]
        public async Task<IActionResult> EditTimeZoneOffset(string userId, string timeZoneOffset)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.TimeZoneOffset = timeZoneOffset;
                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("deposite")]
        public async Task<IActionResult> Deposite(string userId, decimal balance)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Balance += balance;
                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("setLanguage")]
        public async Task<IActionResult> SetLanguage(string userId, string culture)
        {
            ApplicationUser user = await _applicationUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Language += culture;
                var result = await _applicationUserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return NotFound();
        }
    }
}
