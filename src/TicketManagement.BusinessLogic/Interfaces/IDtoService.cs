namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IDtoService<T>
    {
        /// <summary>
        /// Add object to the database.
        /// </summary>
        /// <param name="dto">The object to add to the database.</param>
        void Create(T dto);

        /// <summary>
        /// Delete object from the database.
        /// </summary>
        /// <param name="dto">The object to delete from the database.</param>
        void Delete(T dto);

        /// <summary>
        /// Update object in the database.
        /// </summary>
        /// <param name="dto">The object to update in the database.</param>
        void Update(T dto);
    }
}
