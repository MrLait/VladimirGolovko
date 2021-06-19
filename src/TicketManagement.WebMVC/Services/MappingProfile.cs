using AutoMapper;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.Basket;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;
using TicketManagement.WebMVC.ViewModels.EventViewModels;
using TicketManagement.WebMVC.ViewModels.ProfileViewModels;
using ClientBasketItem = TicketManagement.WebMVC.Clients.Basket.BasketItem;
using ViewBasketItem = TicketManagement.WebMVC.ViewModels.BasketViewModels.BasketItem;

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
            CreateMap<ApplicationUser, ProfileViewModel>();
            CreateMap<ProfileViewModel, ApplicationUser>();
            CreateMap<BasketModel, BasketViewModel>();
            CreateMap<ClientBasketItem, ViewBasketItem>();
            CreateMap<ViewBasketItem, ClientBasketItem>();
        }
    }
}
