using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassicMvc.Models
{
    public interface IJsonSerializer<T>
        where T : class
    {
        IEnumerable<T> DeserializeObjectsFromJson(string filePath);

        Task SerializeObjectsToJsonAsync(IEnumerable<T> models, string filePath);

        string SerializeObjectToJsonString(T model);
    }
}
