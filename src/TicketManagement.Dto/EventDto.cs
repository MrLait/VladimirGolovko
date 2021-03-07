using System;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    public class EventDto : IDtoEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LayoutId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Price { get; set; }

        public int State { get; set; }
    }
}
