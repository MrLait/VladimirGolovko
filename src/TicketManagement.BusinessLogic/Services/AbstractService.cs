using System;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    internal abstract class AbstractService<T> : IDtoService<T>
        where T : IDtoEntity
    {
        protected AbstractService(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; private set; }

        /// <inheritdoc cref="IDtoService{T}"/>
        public abstract void Create(T dto);

        /// <inheritdoc cref="IDtoService{T}"/>
        public abstract void Delete(T dto);

        /// <inheritdoc cref="IDtoService{T}"/>
        public abstract void Update(T dto);

        ////////private void IndexValidation(T dto)
        ////////{
        ////////    if (dto.Id == 0)
        ////////    {
        ////////        throw new ArgumentException($"The dto id can't be equal zero: {dto.Id}");
        ////////    }
        ////////}

        ////////private void DtoNullValidation(T dto)
        ////////{
        ////////    if (Equals(dto, default(T)))
        ////////    {
        ////////        throw new ArgumentException($"The null reference: {dto.GetType().Name}.");
        ////////    }
        ////////}
    }
}
