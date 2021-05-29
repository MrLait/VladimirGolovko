using System;
using System.ComponentModel.DataAnnotations;

namespace ThirdPartyEventEditor.Models
{
    public class ThirdPartyEvent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Description required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image required")]
        public string PosterImage { get; set; }
    }
}