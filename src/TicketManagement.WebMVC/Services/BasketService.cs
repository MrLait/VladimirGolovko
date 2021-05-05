using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels;
using TicketManagement.WebMVC.ViewModels.BasketViewModels;

namespace TicketManagement.WebMVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IEventSeatService _eventSeatService;
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventService _eventService;

        public BasketService(IDbContext dbContext, IEventSeatService eventSeatService, IEventAreaService eventAreaService, IEventService eventService)
        {
            DbContext = dbContext;
            _eventSeatService = eventSeatService;
            _eventAreaService = eventAreaService;
            _eventService = eventService;
        }

        /// <summary>
        /// Gets property database context.
        /// </summary>
        public IDbContext DbContext { get; }

        public async Task AddAsync(ApplicationUser user, int productId)
        {
            var basketItem = new Basket
            {
                ProductId = productId,
                UserId = user.Id,
            };
            await DbContext.Baskets.CreateAsync(basketItem);
        }

        public async Task<IQueryable<Basket>> GetAllAsync()
        {
            return await DbContext.Baskets.GetAllAsync();
        }

        public async Task<BasketViewModel> GetAllByUserAsync(ApplicationUser user)
        {
            var basketItems = (await DbContext.Baskets.GetAllAsync()).Where(x => x.UserId == user.Id);
            var basketViewModel = new BasketViewModel
            {
                UserId = user.Id,
            };
            foreach (var item in basketItems)
            {
                var seatItem = await _eventSeatService.GetByIDAsync(item.ProductId);
                var areaItem = await _eventAreaService.GetByIDAsync(seatItem.EventAreaId);
                var eventItem = await _eventService.GetByIDAsync(areaItem.EventId);
                basketViewModel.Items.Add(
                    new BasketItem
                    {
                        EventName = eventItem.Name,
                        EventAreaDescription = areaItem.Description,
                        Row = seatItem.Row,
                        NumberOfSeat = seatItem.Number,
                        EventDateTimeStart = eventItem.StartDateTime,
                        EventDateTimeEnd = eventItem.EndDateTime,
                        Price = areaItem.Price,
                        PictureUrl = eventItem.ImageUrl,
                    });
            }

            return basketViewModel;
        }

        public async Task DeleteAsync(Basket basketItem)
        {
            if (basketItem.Id == 0)
            {
                var productId = (await GetAllAsync()).Where(x => x.ProductId == basketItem.ProductId && x.UserId == basketItem.UserId).FirstOrDefault().Id;
                basketItem.Id = productId;
                await DbContext.Baskets.DeleteAsync(basketItem);
                return;
            }

            await DbContext.Baskets.DeleteAsync(basketItem);
        }
    }
}
