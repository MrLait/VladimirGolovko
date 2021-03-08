namespace TicketManagement.BusinessLogic.Interfaces
{
    using TicketManagement.DataAccess.Interfaces;

    /// <summary>
    /// Service interface with databese context.
    /// </summary>
    internal interface IService
    {
        IDbContext DbContext { get; }
    }
}
