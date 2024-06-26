﻿namespace TicketManagement.Services.EventFlow.API
{
    /// <summary>
    /// User api Options.
    /// </summary>
    public class ApiOptions
    {
        /// <summary>
        /// Identity api address name in appsettings.
        /// </summary>
        public const string IdentityApiAddressName = "IdentityApiAddress";

        /// <summary>
        /// React app address name in appsettings.
        /// </summary>
        public const string ReactAppAddressName = "ReactAppAddress";

        /// <summary>
        /// Identity api address.
        /// </summary>
        public string IdentityApiAddress { get; set; }

        /// <summary>
        /// React app address.
        /// </summary>
        public string ReactAppAddress { get; set; }
    }
}