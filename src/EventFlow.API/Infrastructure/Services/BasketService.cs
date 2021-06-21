using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Enums;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services
{
    internal class BasketService : IBasketService
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

        /// <inheritdoc/>
        public async Task AddAsync(string userId, int productId)
        {
            var basketItem = new Basket
            {
                ProductId = productId,
                UserId = userId,
            };
            await DbContext.Baskets.CreateAsync(basketItem);
        }

        /// <inheritdoc/>
        public IEnumerable<Basket> GetAll()
        {
            return DbContext.Baskets.GetAllAsQueryable();
        }

        /// <inheritdoc/>
        public async Task<BasketModel> GetAllByUserIdAsync(string id)
        {
            var basketItems = DbContext.Baskets.GetAllAsQueryable().Where(x => x.UserId == id);
            var basketModel = new BasketModel
            {
                UserId = id,
            };
            foreach (var item in basketItems.ToList())
            {
                var seatItem = await _eventSeatService.GetByIDAsync(item.ProductId);
                var areaItem = await _eventAreaService.GetByIDAsync(seatItem.EventAreaId);
                var eventItem = await _eventService.GetByIDAsync(areaItem.EventId);
                basketModel.Items.Add(
                    new BasketItem
                    {
                        Id = seatItem.Id.ToString(),
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

            return basketModel;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string userId, int productId)
        {
            var currentSeatState = (await _eventSeatService.GetByIDAsync(productId)).State;
            if (currentSeatState == States.Purchased)
            {
                return;
            }

            var products = GetAll().Where(x => x.ProductId == productId && x.UserId == userId);

            var productInCurrentUserBasket = !products.Any();
            if (productInCurrentUserBasket)
            {
                throw new ValidationException(ExceptionMessages.ProductInAnotherUserBusket);
            }

            var basketId = products.FirstOrDefault().Id;
            await DbContext.Baskets.DeleteAsync(new Basket { Id = basketId, ProductId = productId, UserId = userId });
        }

        /// <inheritdoc/>
        public async Task DeleteAllByUserIdAsync(string id)
        {
            var basketItems = DbContext.Baskets.GetAllAsQueryable().Where(x => x.UserId == id);
            foreach (var item in basketItems.ToList())
            {
                await DbContext.Baskets.DeleteAsync(new Basket { Id = item.Id, ProductId = item.ProductId, UserId = item.UserId });
            }
        }
    }
}
