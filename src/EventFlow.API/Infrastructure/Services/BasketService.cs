using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Enums;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.EventFlow.API.Models;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.Services
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

        public async Task AddAsync(string userId, int productId)
        {
            var basketItem = new Basket
            {
                ProductId = productId,
                UserId = userId,
            };
            await DbContext.Baskets.CreateAsync(basketItem);
        }

        ////public IQueryable<Basket> GetAll()
        ////{
        ////    return DbContext.Baskets.GetAllAsQueryable();
        ////}

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

        ////public async Task DeleteAsync(Basket basketItem)
        ////{
        ////    if (basketItem.Id == 0)
        ////    {
        ////        var currentSeatState = (await _eventSeatService.GetByIDAsync(basketItem.ProductId)).State;
        ////        if (currentSeatState == States.Purchased)
        ////        {
        ////            return;
        ////        }

        ////        var productId = GetAll().Where(x => x.ProductId == basketItem.ProductId && x.UserId == basketItem.UserId).FirstOrDefault().Id;
        ////        basketItem.Id = productId;
        ////        await DbContext.Baskets.DeleteAsync(basketItem);
        ////        return;
        ////    }

        ////    await DbContext.Baskets.DeleteAsync(basketItem);
        ////}

        ////public async Task DeleteAsync(ApplicationUser user)
        ////{
        ////    var basketItems = DbContext.Baskets.GetAllAsQueryable().Where(x => x.UserId == user.Id);
        ////    foreach (var item in basketItems.ToList())
        ////    {
        ////        await DeleteAsync(item);
        ////    }
        ////}
    }
}
