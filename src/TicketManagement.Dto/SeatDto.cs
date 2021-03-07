using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    public class SeatDto : IDtoEntity
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
