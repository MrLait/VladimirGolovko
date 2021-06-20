using AutoMapper;
using Identity.API.Models;
using TicketManagement.Services.Identity.Domain.Models;

namespace Identity.API.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ////CreateMap<EventAreaDto, EventAreaItem>();
            ////CreateMap<EventAreaItem, EventAreaDto>();
            ////CreateMap<EventViewModel, EventDto>();
            ////CreateMap<EventDto, EventViewModel>();
            CreateMap<RegisterModel, ApplicationUser>();
            ////CreateMap<ApplicationUser, ProfileViewModel>();
            ////CreateMap<ProfileViewModel, ApplicationUser>();
        }
    }
}
