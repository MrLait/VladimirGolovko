using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// Seat data transfer object class.
    /// </summary>
    public class SeatDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets AreaId.
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// Gets or sets Row.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets Number.
        /// </summary>
        public int Number { get; set; }
    }
}
