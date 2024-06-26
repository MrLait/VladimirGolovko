﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ClassicMvc.Services
{
    public class JsonSerializerService<T> : IJsonSerializerService<T>
        where T : class
    {
        public IEnumerable<T> DeserializeObjectsFromJson(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var models = JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
            return models;
        }

        public void SerializeObjectsToJson(IEnumerable<T> models, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(models);
            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(jsonData);
            }
        }

        public string SerializeObjectToJsonString(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}