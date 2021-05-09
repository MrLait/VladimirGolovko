using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.WebMVC.Models
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
            await AddRole(roleManager, "admin");
            await AddRole(roleManager, "eventManager");
            await AddRole(roleManager, "user");

            await AddUser(userManager, roleName: "admin", adminEmail, password);
            await AddUser(userManager, roleName: "eventManager", eventManagerEmail, password);
            await AddUser(userManager, roleName: "user", firstUserEmail, password);
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
