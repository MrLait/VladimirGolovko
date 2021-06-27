using TicketManagement.DataAccess.Domain.Enums;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// EventSeat data transfer object class.
    /// </summary>
    public class EventSeatDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets EventAreaId.
        /// </summary>
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets Row.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets Number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public States State { get; set; }
    }
}
