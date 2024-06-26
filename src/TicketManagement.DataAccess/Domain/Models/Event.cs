﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.DataAccess.Domain.Interfaces;

namespace TicketManagement.DataAccess.Domain.Models
{
    /// <summary>
    /// Event table.
    /// </summary>
    [Table("Event")]
    public class Event : IEntity
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name column in table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description column in table.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layoutId column in table.
        /// </summary>
        [ForeignKey("LayoutId")]
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets start date time column in table.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets end date time column in table.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets image url column in table.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
