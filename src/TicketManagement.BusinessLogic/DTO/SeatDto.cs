using System;
using System.Collections.Generic;
using System.Text;

namespace TicketManagement.BusinessLogic.DTO
{
    internal class SeatDto
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
