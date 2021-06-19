////using System.Linq;
////using System.Threading.Tasks;
////using TicketManagement.BusinessLogic.Interfaces;
////using TicketManagement.DataAccess.Enums;
////using TicketManagement.DataAccess.Interfaces;
////using TicketManagement.Services.Basket.API.Models;
////using BasketModel = TicketManagement.DataAccess.Domain.Models.Basket;

////namespace TicketManagement.Services.Basket.API.Services
////{
////    public class BasketService : IBasketService
////    {
////        private readonly IEventSeatService _eventSeatService;
////        private readonly IEventAreaService _eventAreaService;
////        private readonly IEventService _eventService;

////        public BasketService(IDbContext dbContext, IEventSeatService eventSeatService, IEventAreaService eventAreaService, IEventService eventService)
////        {
////            DbContext = dbContext;
////            _eventSeatService = eventSeatService;
////            _eventAreaService = eventAreaService;
////            _eventService = eventService;
////        }

////        /// <summary>
////        /// Gets property database context.
////        /// </summary>
////        public IDbContext DbContext { get; }

////        public async Task AddAsync(ApplicationUser user, int productId)
////        {
////            var basketItem = new BasketModel
////            {
////                ProductId = productId,
////                UserId = user.Id,
////            };
////            await DbContext.Baskets.CreateAsync(basketItem);
////        }

////        public IQueryable<BasketModel> GetAll()
////        {
////            return DbContext.Baskets.GetAllAsQueryable();
////        }

////        public async Task<BasketViewModel> GetAllByUserAsync(ApplicationUser user)
////        {
////            var basketItems = DbContext.Baskets.GetAllAsQueryable().Where(x => x.UserId == user.Id);
////            var basketViewModel = new BasketViewModel
////            {
////                UserId = user.Id,
////            };
////            foreach (var item in basketItems.ToList())
////            {
////                var seatItem = await _eventSeatService.GetByIDAsync(item.ProductId);
////                var areaItem = await _eventAreaService.GetByIDAsync(seatItem.EventAreaId);
////                var eventItem = await _eventService.GetByIDAsync(areaItem.EventId);
////                basketViewModel.Items.Add(
////                    new BasketItem
////                    {
////                        EventName = eventItem.Name,
////                        EventAreaDescription = areaItem.Description,
////                        Row = seatItem.Row,
////                        NumberOfSeat = seatItem.Number,
////                        EventDateTimeStart = eventItem.StartDateTime,
////                        EventDateTimeEnd = eventItem.EndDateTime,
////                        Price = areaItem.Price,
////                        PictureUrl = eventItem.ImageUrl,
////                    });
////            }
////            return basketViewModel;
////        }

////        public async Task DeleteAsync(BasketModel basketItem)
////        {
////            if (basketItem.Id == 0)
////            {
////                var currentSeatState = (await _eventSeatService.GetByIDAsync(basketItem.ProductId)).State;
////                if (currentSeatState == States.Purchased)
////                {
////                    return;
////                }

////                var productId = GetAll().Where(x => x.ProductId == basketItem.ProductId && x.UserId == basketItem.UserId).FirstOrDefault().Id;
////                basketItem.Id = productId;
////                await DbContext.Baskets.DeleteAsync(basketItem);
////                return;
////            }

////            await DbContext.Baskets.DeleteAsync(basketItem);
////        }

////        public async Task DeleteAsync(ApplicationUser user)
////        {
////            var basketItems = DbContext.Baskets.GetAllAsQueryable().Where(x => x.UserId == user.Id);
////            foreach (var item in basketItems.ToList())
////            {
////                await DeleteAsync(item);
////            }
////        }
////    }
////}
