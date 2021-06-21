using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketManagement.Services.Identity.API.Extensions;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.Controllers
{
    [Route("[controller]")]
    public class AccountUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly JwtTokenService _jwtTokenService;

        public AccountUserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">Register model.</param>
        /// <returns>Returns status codes;
        /// Errors:
        /// PasswordTooShort,
        /// PasswordRequiresNonAlphanumeric,
        /// PasswordRequiresDigit,
        /// PasswordRequiresUpperstring.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
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
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            string resultError = string.Empty;
            resultError = resultError.ConverIdentityResultErrorToString(result);
            return BadRequest(resultError);
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="model">Login model.</param>
        /// <returns>Token or Json witn Identity.SignInResult result.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            string resultError = JsonConvert.SerializeObject(result);
            return BadRequest(resultError);
        }

        /// <summary>
        /// Validate token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>Return token.</returns>
        [HttpGet("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Validate(string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok() : (IActionResult)Forbid();
        }
    }
}
