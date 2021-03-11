using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    /// <summary>
    /// Layout table.
    /// </summary>
    [Table("Layout")]
    public class Layout : IEntity
    {
        /// <summary>
        /// Id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// VenueId column in table.
        /// </summary>
        [ForeignKey("VenueId")]
        public int VenueId { get; set; }

        /// <summary>
        /// Description column in table.
        /// </summary>
        public string Description { get; set; }
    }
}
