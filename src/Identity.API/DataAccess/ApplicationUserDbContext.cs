using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.DataAccess
{
    /// <summary>
    /// Application user db context.
    /// </summary>
    public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserDbContext"/> class.
        /// asd.
        /// </summary>
        /// <param name="options">Db context options.</param>
        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options)
            : base(options)
        {
        }
    }
}
