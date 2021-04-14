namespace TicketManagement.IntegrationTests.Services.Interfaces
{
    internal interface IExecuteScript
    {
        void ExecuteScript(string sqlConnectionString, string script);
    }
}
