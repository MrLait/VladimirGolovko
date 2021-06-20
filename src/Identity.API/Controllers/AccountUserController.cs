using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<RegisterModel, ApplicationUser>(model);
                user.UserName = model.Email;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    var roles = await _userManager.GetRolesAsync(user);
                    return Ok(_jwtTokenService.GetToken(user, roles));
                }

                var errorList = new List<string>();
                foreach (var error in result.Errors)
                {
                    errorList.Add(error.Code);
                }

                var resultError = string.Empty;
                resultError += string.Join(',', errorList);

                return BadRequest(resultError);
            }

            return BadRequest("ModelState is not valid");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    return Ok(_jwtTokenService.GetToken(user, roles));
                }
            }

            return Forbid();
        }

        [HttpGet("validate")]
        public IActionResult Validate(string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok() : (IActionResult)Forbid();
        }
    }
}
