using System.ComponentModel.DataAnnotations;

namespace TicketManagement.DataAccess.Domain.Interfaces
{
    /// <summary>
    /// Entity interface with Id contract.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        int Id { get; set; }
    }
}
