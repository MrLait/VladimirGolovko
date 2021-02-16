using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    public class EventSeat : IEntity
    {
        public int Id { get; set; }

        public int EventAreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public int State { get; set; }
    }
}
