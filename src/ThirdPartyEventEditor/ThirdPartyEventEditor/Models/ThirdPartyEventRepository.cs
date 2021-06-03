using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Services
{
    public class ThirdPartyEventRepository : IThirdPartyEventRepository
    {
        private static readonly Mutex _mutexObj = new Mutex();
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);
        private readonly IJsonSerializerService<ThirdPartyEvent> _jsonSerializer;

        public ThirdPartyEventRepository(IJsonSerializerService<ThirdPartyEvent> jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public void Create(ThirdPartyEvent thirdPartyEvent)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            thirdPartyEvent.Id = deserThirdPartyEvent.LastOrDefault()?.Id + 1 ?? 1;
            deserThirdPartyEvent.Add(thirdPartyEvent);
            _mutexObj.WaitOne();
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
            _mutexObj.ReleaseMutex();
        }

        public void Delete(int id)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == id);
            deserThirdPartyEvent.RemoveAt(thirdPartyEventIndex);
            _mutexObj.WaitOne();
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
            _mutexObj.ReleaseMutex();
        }

        public IEnumerable<ThirdPartyEvent> GetAll()
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            return deserThirdPartyEvent;
        }

        public void Update(ThirdPartyEvent thirdPartyEvent)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == thirdPartyEvent.Id);
            deserThirdPartyEvent[thirdPartyEventIndex] = thirdPartyEvent;
            _mutexObj.WaitOne();
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
            _mutexObj.ReleaseMutex();
        }

        public ThirdPartyEvent GetById(int id)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == id);
            var thirdPartyEvent = deserThirdPartyEvent.ElementAt(thirdPartyEventIndex);
            return thirdPartyEvent;
        }
    }
}