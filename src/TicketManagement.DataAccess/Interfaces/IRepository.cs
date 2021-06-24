using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// CRUD operations repository interface.
    /// </summary>
    /// <typeparam name="T">Table model.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Get object by Id from table in database.
        /// </summary>
        /// <param name="byId">Object id.</param>
        /// <returns>Returns object by id.</returns>
        Task<T> GetByIdAsync(int byId);

        /// <summary>
        /// Add object to database.
        /// </summary>
        /// <param name="entity">Object to add in database.</param>
        Task CreateAsync(T entity);

        /// <summary>
        /// Delete object from table by Id.
        /// </summary>
        /// <param name="entity">Entity.</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Modify an existing object.
        /// </summary>
        /// <param name="entity">Object with parameters to be changed.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Method to get all objects from database table.
        /// </summary>
        /// <returns>Returns list of objects.</returns>
        IQueryable<T> GetAllAsQueryable();
    }
}
