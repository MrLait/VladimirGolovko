using System.Collections.Generic;
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

        public void SerializeObjectsToJson(IEnumerable<T> models, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(models);
            System.IO.File.WriteAllText(filePath, jsonData);
        }

        public string SerializeObjectToJsonString(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}