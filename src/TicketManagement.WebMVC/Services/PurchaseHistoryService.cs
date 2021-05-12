using System;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels;
using TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels;

namespace TicketManagement.WebMVC.Services
{
    public class PurchaseHistoryService : IPurchaseHistoryService
    {
        private readonly IEventSeatService _eventSeatService;
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventService _eventService;

        public PurchaseHistoryService(IDbContext dbContext, IEventSeatService eventSeatService, IEventAreaService eventAreaService, IEventService eventService)
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
            var purchaseHistoryItem = new PurchaseHistory
            {
                ProductId = productId,
                UserId = user.Id,
            };

            await DbContext.PurchaseHistories.CreateAsync(purchaseHistoryItem);
        }

        public async Task<PurchaseHistoryViewModel> GetAllByUserAsync(ApplicationUser user)
        {
            var purchaseHistoryItems = DbContext.PurchaseHistories.GetAllAsQueryable().Where(x => x.UserId == user.Id);
            var purchaseHistoryViewModel = new PurchaseHistoryViewModel
            {
                UserId = user.Id,
            };
            foreach (var item in purchaseHistoryItems.ToList())
            {
                var seatItem = await _eventSeatService.GetByIDAsync(item.ProductId);
                var areaItem = await _eventAreaService.GetByIDAsync(seatItem.EventAreaId);
                var eventItem = await _eventService.GetByIDAsync(areaItem.EventId);
                purchaseHistoryViewModel.Items.Add(
                    new PurchaseHistoryItem
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

            return purchaseHistoryViewModel;
        }

        public async Task AddFromBasketAsync(IQueryable<Basket> baskets)
        {
            foreach (var item in baskets.ToList())
            {
                var purchaseHistoryItem = new PurchaseHistory
                {
                    ProductId = item.ProductId,
                    UserId = item.UserId,
                };

                await DbContext.PurchaseHistories.CreateAsync(purchaseHistoryItem);
            }
        }

        public Task DeleteAsync(PurchaseHistory purchaseHistory)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<PurchaseHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
