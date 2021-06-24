using AutoMapper;
using TicketManagement.Services.Identity.API.Models;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.Identity.API.Infrastructure.Services
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<RegisterModel, ApplicationUser>();
        }
    }
}
