using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class LayoutRepository : AdoUsingParametersRepository<Layout>
    {
        public LayoutRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
