using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class EventSeatRepository : AdoUsingParametersRepository<EventSeat>
    {
        public EventSeatRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
