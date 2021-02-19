using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    [Table("Layout")]
    public class Layout : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("VenueId")]
        public int VenueId { get; set; }

        public string Description { get; set; }
    }
}
