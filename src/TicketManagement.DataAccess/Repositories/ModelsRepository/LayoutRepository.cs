using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

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
