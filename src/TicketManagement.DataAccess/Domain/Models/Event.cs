using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    [Table("Venue")]
    public class Event : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LayoutId { get; set; }
    }
}
