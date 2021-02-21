using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

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
