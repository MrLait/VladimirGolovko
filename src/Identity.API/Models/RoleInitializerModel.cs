using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Services.Identity.Domain.Models;

namespace Identity.API.Models
{
    public class RoleInitializerModel
    {
        protected RoleInitializerModel()
        {
        }

        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string eventManagerEmail = "eventManager@gmail.com";
            string firstUserEmail = "firstUser@gmail.com";
            string password = "_Aa123456";
            await AddRole(roleManager, UserRoles.Admin);
            await AddRole(roleManager, UserRoles.EventManager);
            await AddRole(roleManager, UserRoles.User);

            await AddUser(userManager, roleName: UserRoles.Admin, adminEmail, password);
            await AddUser(userManager, roleName: UserRoles.EventManager, eventManagerEmail, password);
            await AddUser(userManager, roleName: UserRoles.User, firstUserEmail, password);
        }

        private static async Task AddUser(UserManager<ApplicationUser> userManager, string roleName, string adminEmail, string password)
        {
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser user = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }

                await userManager.AddClaimAsync(user, CreateClaim("TimeZoneOffset", "0"));
            }
        }

        private static async Task AddRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}
