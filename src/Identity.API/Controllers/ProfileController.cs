using System.Collections.Generic;
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
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            UserManager<ApplicationUser> applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
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
    }
}
