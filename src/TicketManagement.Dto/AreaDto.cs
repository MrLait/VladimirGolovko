namespace TicketManagement.Dto
{
    using TicketManagement.Dto.Interfaces;

    /// <summary>
    /// Area data transfer object class.
    /// </summary>
    internal class AreaDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LayoutId.
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets coordY.
        /// </summary>
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets coordY.
        /// </summary>
        public int CoordY { get; set; }
    }
}
