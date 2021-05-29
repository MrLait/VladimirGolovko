using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.Models
{
    public class ThirdPartyEvent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string PosterImage { get; set; }
    }
}
