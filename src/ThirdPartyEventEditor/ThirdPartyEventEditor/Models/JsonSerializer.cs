using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClassicMvc.Models
{
    public class JsonSerializer<T> : IJsonSerializer<T>
        where T : class
    {
        public IEnumerable<T> DeserializeObjectsFromJson(string filePath)
        {
            var jsonData = System.IO.File.ReadAllText(filePath);
            var models = JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
            return models;
        }

        public async Task SerializeObjectsToJsonAsync(IEnumerable<T> models, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(models);

            using (var sw = new StreamWriter(filePath))
            {
                await sw.WriteAsync(jsonData);
            }
        }

        public string SerializeObjectToJsonString(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}