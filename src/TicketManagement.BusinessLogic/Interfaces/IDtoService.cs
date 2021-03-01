namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface IDtoService<T>
    {
        void Create(T dto);
    }
}
