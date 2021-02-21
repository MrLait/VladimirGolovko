using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

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
