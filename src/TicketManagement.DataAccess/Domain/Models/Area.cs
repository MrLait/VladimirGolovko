using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    /// <summary>
    /// Area table.
    /// </summary>
    [Table("Area")]
    public class Area : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets layoutId column in table.
        /// </summary>
        [ForeignKey("LayoutId")]
        public int LayoutId { get; set; }

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
    }
}
