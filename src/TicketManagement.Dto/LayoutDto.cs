using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    internal class LayoutDto : IDtoEntity
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string Description { get; set; }
    }
}
