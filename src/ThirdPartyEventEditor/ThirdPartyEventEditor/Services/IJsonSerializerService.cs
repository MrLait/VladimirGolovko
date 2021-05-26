using System.Collections.Generic;

namespace ClassicMvc.Services
{
    public interface IJsonSerializerService<T>
        where T : class
    {
        IEnumerable<T> DeserializeObjectsFromJson(string filePath);

        void SerializeObjectsToJson(IEnumerable<T> models, string filePath);

        string SerializeObjectToJsonString(T model);
    }
}
