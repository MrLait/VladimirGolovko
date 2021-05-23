using System;
using System.ComponentModel.DataAnnotations;

namespace ThirdPartyEventEditor.Models
{
    public class ThirdPartyEvent
    {
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string PosterImage { get; set; }
    }
}