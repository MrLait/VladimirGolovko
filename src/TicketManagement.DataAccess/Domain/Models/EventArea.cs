using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    /// <summary>
    /// EventArea table.
    /// </summary>
    [Table("EventArea")]
    public class EventArea : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets eventId column in table.
        /// </summary>
        [ForeignKey("EventId")]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets description column in table.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets coordX column in table.
        /// </summary>
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets coordY column in table.
        /// </summary>
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets price column in table.
        /// </summary>
        public decimal Price { get; set; }
    }
}
