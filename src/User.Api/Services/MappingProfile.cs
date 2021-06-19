using AutoMapper;
using TicketManagement.Services.User.API.Models;

namespace TicketManagement.Services.User.API.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ////CreateMap<EventAreaDto, EventAreaItem>();
            ////CreateMap<EventAreaItem, EventAreaDto>();
            ////CreateMap<EventViewModel, EventDto>();
            ////CreateMap<EventDto, EventViewModel>();
            ////CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<ApplicationUser, ProfileViewModel>();
            ////CreateMap<ProfileViewModel, ApplicationUser>();
        }
    }
}
