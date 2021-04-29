using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Interfaces;
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

        protected EfDbContext Context { get; set; }

        public async Task CreateAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Context.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            var test = await Context.Set<T>().ToListAsync();
            return test.AsQueryable();
        }

        public async Task<T> GetByIDAsync(int byId)
        {
            return await Context.Set<T>().FindAsync(byId);
        }

        public Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChangesAsync();
        }
    }
}
