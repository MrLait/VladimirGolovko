using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Models
{
    public class ThirdPartyEventRepository : IThirdPartyEventRepository
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);

        public void Create(ThirdPartyEvent thirdPartyEvent)
        {
            DeserializeObjectsFromJson(out List<ThirdPartyEvent> thirdPartyEventList);

            thirdPartyEvent.Id = thirdPartyEventList.LastOrDefault()?.Id + 1 ?? 1;
            thirdPartyEventList.Add(thirdPartyEvent);
            SerializeObjectsToJson(thirdPartyEventList);
        }

        public void Delete(int id)
        {
            DeserializeObjectsFromJson(out List<ThirdPartyEvent> thirdPartyEventList);
            var thirdPartyEventIndex = thirdPartyEventList.FindIndex(x => x.Id == id);
            thirdPartyEventList.RemoveAt(thirdPartyEventIndex);
            SerializeObjectsToJson(thirdPartyEventList);
        }

        public IEnumerable<ThirdPartyEvent> GetAll()
        {
            DeserializeObjectsFromJson(out List<ThirdPartyEvent> thirdPartyEventList);
            return thirdPartyEventList;
        }

        public void Update(ThirdPartyEvent thirdPartyEvent)
        {
            DeserializeObjectsFromJson(out List<ThirdPartyEvent> thirdPartyEventList);
            var thirdPartyEventIndex = thirdPartyEventList.FindIndex(x => x.Id == thirdPartyEvent.Id);
            thirdPartyEventList[thirdPartyEventIndex] = thirdPartyEvent;
            SerializeObjectsToJson(thirdPartyEventList);
        }

        public ThirdPartyEvent GetById(int id)
        {
            DeserializeObjectsFromJson(out List<ThirdPartyEvent> thirdPartyEventList);
            var thirdPartyEventIndex = thirdPartyEventList.FindIndex(x => x.Id == id);
            var thirdPartyEvent = thirdPartyEventList.ElementAt(thirdPartyEventIndex);
            return thirdPartyEvent;
        }

        private void DeserializeObjectsFromJson(out List<ThirdPartyEvent> thirdPartyEventList)
        {
            var jsonData = System.IO.File.ReadAllText(_filePath);
            thirdPartyEventList = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(jsonData) ?? new List<ThirdPartyEvent>();
        }

        private void SerializeObjectsToJson(IEnumerable<ThirdPartyEvent> thirdPartyEventList)
        {
            var jsonData = JsonConvert.SerializeObject(thirdPartyEventList);
            System.IO.File.WriteAllText(_filePath, jsonData);
        }
    }
}