using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class SeatRepository : AdoUsingParametersRepository<Seat>
    {
        public SeatRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
