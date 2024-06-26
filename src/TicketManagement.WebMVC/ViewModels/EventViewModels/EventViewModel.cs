﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.ViewModels.EventViewModels
{
    /// <summary>
    /// Event view model.
    /// </summary>
    public class EventViewModel
    {
        /// <summary>
        /// Gets or sets id property.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name column in table.
        /// </summary>
        [Required(ErrorMessage = "NameRequired")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description column in table.
        /// </summary>
        [Required(ErrorMessage = "DescriptionRequired")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layoutId column in table.
        /// </summary>
        [Required(ErrorMessage = "LayoutRequired")]
        [Display(Name = "LayoutId")]
        public int? LayoutId { get; set; }

        /// <summary>
        /// Gets or sets start date time column in table.
        /// </summary>
        [Required(ErrorMessage = "StartTimeRequired")]
        [Display(Name = "StartDateTime")]
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets end date time column in table.
        /// </summary>
        [Required(ErrorMessage = "EndTimeRequired")]
        [Display(Name = "EndDateTime")]
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets image url column in table.
        /// </summary>
        [Required(ErrorMessage = "ImageUrlRequired")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; init; }

        /// <summary>
        /// Event area items.
        /// </summary>
        public List<EventAreaItem> EventAreaItems { get; set; } = new ();
    }
}
