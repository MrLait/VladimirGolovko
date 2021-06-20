using AutoMapper;
using Identity.API.Models;
using TicketManagement.Services.Identity.Domain.Models;

namespace Identity.API.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, ApplicationUser>();
        }
    }
}
