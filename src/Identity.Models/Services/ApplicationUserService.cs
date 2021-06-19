////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Identity;
////using TicketManagement.Services.EventFlow.API.Models;

////namespace TicketManagement.Services.Identity.Domain.Services
////{
////    public class ApplicationUserService : IApplicationUserService
////    {
////        private readonly UserManager<ApplicationUser> _applicationUserManager;

////        public ApplicationUserService(UserManager<ApplicationUser> applicationUser)
////        {
////            _applicationUserManager = applicationUser;
////        }

////        public async Task<decimal> GetBalanceAsync(ApplicationUser applicationUser)
////        {
////            return (await _applicationUserManager.FindByIdAsync(applicationUser.Id)).Balance;
////        }

////        public async Task UpdateBalanceAsync(ApplicationUser applicationUser)
////        {
////            var user = await _applicationUserManager.FindByIdAsync(applicationUser.Id);
////            user.Balance = applicationUser.Balance;
////            await _applicationUserManager.UpdateAsync(user);
////        }
////    }
////}
