using System.Threading.Tasks;
using AutoMapper;
using Identity.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services.Identity.API.Extensions;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">Register model.</param>
        /// <returns>Returns status codes;
        /// Errors:
        /// PasswordTooShort,
        /// PasswordRequiresNonAlphanumeric,
        /// PasswordRequiresDigit,
        /// PasswordRequiresUpperstring.</returns>
        [HttpPost("createUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateUser([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            var user = _mapper.Map<RegisterModel, ApplicationUser>(model);
            user.UserName = model.Email;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
                return Ok();
            }

            string resultError = string.Empty;
            resultError = resultError.ConverIdentityResultErrorToString(result);

            return BadRequest(resultError);
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="model">Register model.</param>
        /// <returns>Returns status code.</returns>
        [HttpPut("updateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Forbid("ModelStateIsNotValid");
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var tempUserId = user.Id;
            var curUser = _mapper.Map<RegisterModel, ApplicationUser>(model);

            user = curUser;
            user.Id = tempUserId;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Return user or status code.</returns>
        [HttpGet("getUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            return Ok(user);
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Return status code.</returns>
        [HttpDelete("deleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            await _userManager.DeleteAsync(user);
            return Ok();
        }
    }
}
