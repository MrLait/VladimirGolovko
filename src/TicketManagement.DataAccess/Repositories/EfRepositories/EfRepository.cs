using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.EfRepositories
{
    internal class EfRepository<T> : IRepository<T>
        where T : class
    {
        public EfRepository(EfDbContext context)
        {
            Context = context;
        }

        protected EfDbContext Context { get; }

        public async Task CreateAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return Context.Set<T>().AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int byId)
        {
            return await Context.Set<T>().FindAsync(byId);
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
        }
    }
}
