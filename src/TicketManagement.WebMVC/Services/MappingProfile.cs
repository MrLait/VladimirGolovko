using AutoMapper;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventAreaDto, EventAreaItem>();
            CreateMap<EventAreaItem, EventAreaDto>();
            CreateMap<EventViewModel, EventDto>();
            CreateMap<EventDto, EventViewModel>();
            CreateMap<RegisterViewModel, ApplicationUser>();
        }
    }
}
