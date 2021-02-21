using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class EventAreaRepository : AdoUsingParametersRepository<EventArea>
    {
        public EventAreaRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
