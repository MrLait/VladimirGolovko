using System.Threading.Tasks;
using Identity.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.Controllers
{
    /// <summary>
    /// User profile controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;

        public ProfileController(UserManager<ApplicationUser> applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        /// <summary>
        /// Get user balance.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Return balance.</returns>
        [HttpGet("getBalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetBalanceAsync(string userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            return Ok(user.Balance);
        }

        /// <summary>
        /// Update user balance.
        /// </summary>
        /// <param name="model">Balance model.</param>
        /// <returns>Returns balance.</returns>
        [HttpPut("updateBalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateBalanceAsync([FromBody] BalanceModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            user.Balance = model.Balance;
            await _applicationUserManager.UpdateAsync(user);
            return Ok();
        }

        /// <summary>
        /// Get user profile.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Rerurns user.</returns>
        [HttpGet("getUserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            return Ok(user);
        }

        /// <summary>
        /// Edit first name.
        /// </summary>
        /// <param name="model"> First name model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("editFirstName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditFirstName([FromBody] FirstNameModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.FirstName = model.FirstName;
            var result = await _applicationUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit surname model.
        /// </summary>
        /// <param name="model">Surname model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("editSurname")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditSurname([FromBody] SurnameModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Surname = model.Surname;
            var result = await _applicationUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit email.
        /// </summary>
        /// <param name="model">Edit email model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("editEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditEmail([FromBody] EmailModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Email = model.Email;
            var result = await _applicationUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit password.
        /// </summary>
        /// <param name="model">Password model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("editPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditPassword([FromBody] PasswordModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _applicationUserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit time zone.
        /// </summary>
        /// <param name="model">Time zone model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("editTimeZoneOffset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditTimeZoneOffset([FromBody] TimeZoneOffsetModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.TimeZoneOffset = model.TimeZoneOffset;
            var result = await _applicationUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Deposite.
        /// </summary>
        /// <param name="model">Deposite model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("deposite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deposite([FromBody] DepositeModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Balance += model.Balance;
            var result = await _applicationUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Set language.
        /// </summary>
        /// <param name="model">Language model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("setLanguage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetLanguage([FromBody] LanguageModel model)
        {
            var user = await _applicationUserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Language = model.Culture;
            var result = await _applicationUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
