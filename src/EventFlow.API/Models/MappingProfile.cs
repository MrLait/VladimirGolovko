using AutoMapper;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventAreaDto, EventAreaItem>();
            CreateMap<EventAreaItem, EventAreaDto>();
            CreateMap<EventViewModel, EventDto>();
            CreateMap<EventDto, EventViewModel>();
            ////CreateMap<RegisterViewModel, ApplicationUser>();
            ////CreateMap<ApplicationUser, ProfileViewModel>();
            ////CreateMap<ProfileViewModel, ApplicationUser>();
            ////CreateMap<BasketModel, BasketViewModel>();
            ////CreateMap<ClientBasketItem, ViewBasketItem>();
            ////CreateMap<ViewBasketItem, ClientBasketItem>();
            ////CreateMap<PurchaseHistoryModel, PurchaseHistoryViewModel>();
            ////CreateMap<ClientPurchaseHistoryItem, ViewPurchaseHistoryItem>();
            ////CreateMap<ViewPurchaseHistoryItem, ClientPurchaseHistoryItem>();
        }
    }
}
