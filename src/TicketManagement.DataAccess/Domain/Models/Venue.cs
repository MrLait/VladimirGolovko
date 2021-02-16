using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    public class Venue : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
