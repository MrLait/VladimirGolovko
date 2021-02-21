using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

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
