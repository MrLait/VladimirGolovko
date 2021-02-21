using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.DataAccess.Repositories.ModelsRepository
{
    public class VenueRepository : AdoUsingParametersRepository<Venue>
    {
        public VenueRepository(string dbConString)
            : base(dbConString)
        {
        }
    }
}
