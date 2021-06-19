namespace TicketManagement.WebMVC.Clients.EventFlowClient
{
    public static class EventFlowApiRequestUries
    {
        public const string EventGetAll = "/Event/getAll";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventAreaGetAllByEventId = "/EventArea/getAllByEventId?id={0}";
    }
}
