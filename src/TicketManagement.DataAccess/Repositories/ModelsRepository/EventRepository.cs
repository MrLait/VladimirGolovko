using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class EventRepository : AdoUsingStoredProcedureRepository<Event>
    {
        public EventRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
