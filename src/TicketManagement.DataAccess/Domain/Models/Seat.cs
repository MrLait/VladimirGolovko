using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    /// <summary>
    /// Seat table.
    /// </summary>
    [Table("Seat")]
    public class Seat : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets areaId column in table.
        /// </summary>
        [ForeignKey("AreaId")]
        public int AreaId { get; set; }

        /// <summary>
        /// Gets or sets row column in table.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number column in table.
        /// </summary>
        public int Number { get; set; }
    }
}
