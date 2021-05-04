using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    [Table("Basket")]
    public class Basket : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets ProductId column in table.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets UserId column in table.
        /// </summary>
        public string UserId { get; set; }
    }
}
