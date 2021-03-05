using System;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    internal class EventDto : IDtoEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LayoutId { get; set; }

        public DateTime DateTime { get; set; }

        public string AreaDescription { get; set; }

        public string VenueDescription { get; set; }

        public decimal Price { get; set; }

        public int State { get; set; }
    }
}
