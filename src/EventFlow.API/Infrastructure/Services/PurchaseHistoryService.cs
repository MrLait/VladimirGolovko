using System;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services
{
    internal class PurchaseHistoryService : IPurchaseHistoryService
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

        /// <inheritdoc/>
        public async Task AddAsync(string userId, int itemId)
        {
            var purchaseHistoryItem = new PurchaseHistory
            {
                ProductId = itemId,
                UserId = userId,
            };

            await DbContext.PurchaseHistories.CreateAsync(purchaseHistoryItem);
        }

        /// <inheritdoc/>
        public async Task<PurchaseHistoryModel> GetAllByUserIdAsync(string userId)
        {
            var purchaseHistoryItems = DbContext.PurchaseHistories.GetAllAsQueryable().Where(x => x.UserId == userId);
            var purchaseHistoryViewModel = new PurchaseHistoryModel
            {
                UserId = userId,
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
    }
}
