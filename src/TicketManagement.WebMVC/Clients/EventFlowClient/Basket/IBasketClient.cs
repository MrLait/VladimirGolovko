﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.DataAccess.Enums;
using TicketManagement.WebMVC.Clients.Basket;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Basket
{
    public interface IBasketClient
    {
        public Task<BasketModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default);

        public Task AddToBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default);
    }

    internal class BasketClient : IBasketClient
    {
        private readonly HttpClient _httpClient;

        public BasketClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddToBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.BasketAddToBasket, userId, itemId);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task<BasketModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.BasketGetAllByUserId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var eventAreas = JsonConvert.DeserializeObject<BasketModel>(result);
            return eventAreas;
        }
    }
}