using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

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
