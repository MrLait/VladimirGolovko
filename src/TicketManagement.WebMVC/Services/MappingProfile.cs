using AutoMapper;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.Basket;
using TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory;
using TicketManagement.WebMVC.Clients.IdentityClient.AccountUser;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.AccountViewModels;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;
using TicketManagement.WebMVC.ViewModels.EventViewModels;
using TicketManagement.WebMVC.ViewModels.ProfileViewModels;
using TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels;
using ClientBasketItem = TicketManagement.WebMVC.Clients.EventFlowClient.Basket.BasketItem;
using ClientPurchaseHistoryItem = TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory.PurchaseHistoryItem;
using ViewBasketItem = TicketManagement.WebMVC.ViewModels.BasketViewModels.BasketItem;
using ViewPurchaseHistoryItem = TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels.PurchaseHistoryItem;

namespace TicketManagement.WebMVC.Services
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
            CreateMap<PurchaseHistoryModel, PurchaseHistoryViewModel>();
            CreateMap<ClientPurchaseHistoryItem, ViewPurchaseHistoryItem>();
            CreateMap<ViewPurchaseHistoryItem, ClientPurchaseHistoryItem>();
            CreateMap<RegisterViewModel, RegisterModel>();
            CreateMap<LoginViewModel, LoginModel>();
        }
    }
}
