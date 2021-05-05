using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.WebMVC.ViewModels
{
    public record EventSeatItem
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets EventAreaId.
        /// </summary>
        public int EventAreaId { get; init; }

        /// <summary>
        /// Gets or sets Row.
        /// </summary>
        public int Row { get; init; }

        /// <summary>
        /// Gets or sets Number.
        /// </summary>
        public int Number { get; init; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public int State { get; init; }
    }
}
