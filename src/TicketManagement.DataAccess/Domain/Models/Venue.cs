using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    /// <summary>
    /// Venue table.
    /// </summary>
    [Table("Venue")]
    public class Venue : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets description column in table.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets address column in table.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets phone column in table.
        /// </summary>
        public string Phone { get; set; }
    }
}
