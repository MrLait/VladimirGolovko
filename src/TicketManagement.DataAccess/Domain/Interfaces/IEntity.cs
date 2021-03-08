namespace TicketManagement.DataAccess.Domain.Interfaces
{
    using System.ComponentModel.DataAnnotations;

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
