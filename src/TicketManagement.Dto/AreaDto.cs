using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    internal class AreaDto : IDtoEntity
    {
        public int Id { get; set; }

        public int LayoutId { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}
