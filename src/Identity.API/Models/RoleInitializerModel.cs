using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.Models
{
    /// <summary>
    /// Role initializer model.
    /// </summary>
    public class RoleInitializerModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleInitializerModel"/> class.
        /// </summary>
        protected RoleInitializerModel()
        {
        }

        /// <summary>
        /// Initialize model.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="roleManager">Role manager.</param>
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string adminEmail = "admin@gmail.com";
            const string eventManagerEmail = "eventManager@gmail.com";
            const string firstUserEmail = "firstUser@gmail.com";
            const string password = "_Aa123456";
            await AddRole(roleManager, UserRoles.Admin);
            await AddRole(roleManager, UserRoles.EventManager);
            await AddRole(roleManager, UserRoles.User);

            await AddUser(userManager, roleName: UserRoles.Admin, adminEmail, password);
            await AddUser(userManager, roleName: UserRoles.EventManager, eventManagerEmail, password);
            await AddUser(userManager, roleName: UserRoles.User, firstUserEmail, password);
        }

        private static async Task AddUser(UserManager<ApplicationUser> userManager, string roleName, string email, string password)
        {
            if (await userManager.FindByNameAsync(email) == null)
            {
                var user = new ApplicationUser { Email = email, UserName = email };
                var result = await userManager.CreateAsync(user, password);
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
            return new (type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}
