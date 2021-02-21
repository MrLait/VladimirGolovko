using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

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
