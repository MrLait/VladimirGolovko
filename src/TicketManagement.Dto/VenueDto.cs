using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    public class VenueDto : IDtoEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
