namespace TicketManagement.DataAccess.Domain.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TicketManagement.DataAccess.Domain.Interfaces;

    /// <summary>
    /// EventSeat table.
    /// </summary>
    [Table("EventSeat")]
    public class EventSeat : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets eventAreaId column in table.
        /// </summary>
        [ForeignKey("EventAreaId")]
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets row column in table.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number column in table.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets state column in table.
        /// </summary>
        public int State { get; set; }
    }
}
