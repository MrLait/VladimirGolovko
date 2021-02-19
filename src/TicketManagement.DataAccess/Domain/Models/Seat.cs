using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    [Table("Seat")]
    public class Seat : IEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AreaId")]
        public int AreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
