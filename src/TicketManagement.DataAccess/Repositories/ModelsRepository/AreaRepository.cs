using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class AreaRepository : AdoUsingParametersRepository<Area>
    {
        public AreaRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
