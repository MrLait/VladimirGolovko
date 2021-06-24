﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketManagement.WebMVC.Clients.EventFlowClient.Basket;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventArea;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventManager;
using TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory;
using TicketManagement.WebMVC.Clients.IdentityClient.AccountUser;
using TicketManagement.WebMVC.Clients.IdentityClient.Profile;

namespace TicketManagement.WebMVC.Extensions
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add event flow clients.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
#pragma warning disable S1541 // Methods and properties should not be too complex
        public static void AddEventFlowClients(this IServiceCollection services, IConfiguration configuration)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            services.AddHttpClient<IEventClient, EventClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IEventAreaClient, EventAreaClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IBasketClient, BasketClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IPurchaseHistoryClient, PurchaseHistoryClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IEventManagerClient, EventManagerClient>((provider, client) =>
            {
                var eventFlowApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.EventFlowApiAddress;
                client.BaseAddress = new Uri(eventFlowApiAddress ?? string.Empty);
            });
        }

        /// <summary>
        /// Add identity clients.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddIdentityClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IUserClient, UserClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.IdentityApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
            });
            services.AddHttpClient<IProfileClient, ProfileClient>((provider, client) =>
            {
                var userApiAddress = provider.GetService<IOptions<EventFlowApiOptions>>()?.Value.IdentityApiAddress;
                client.BaseAddress = new Uri(userApiAddress ?? string.Empty);
            });
        }
    }
}
