using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TicketManagement.WebMVC.Services
{
    public interface IJsonSerializerService<T>
        where T : class
    {
        IEnumerable<T> DeserializeObjectsFromJson(string filePath);

        IEnumerable<T> DeserializeObjectsFromString(string data);

        void SerializeObjectsToJson(IEnumerable<T> models, string filePath);

        string SerializeObjectToJsonString(T model);
    }
}
