using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    public class Layout : IEntity
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string Description { get; set; }
    }
}
