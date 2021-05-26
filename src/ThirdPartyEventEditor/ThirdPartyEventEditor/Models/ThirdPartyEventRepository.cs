using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassicMvc.Infrastructure.Utils;
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

        public void CreateAsync(ThirdPartyEvent thirdPartyEvent)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            thirdPartyEvent.Id = deserThirdPartyEvent.LastOrDefault()?.Id + 1 ?? 1;
            deserThirdPartyEvent.Add(thirdPartyEvent);
            _mutexObj.WaitOne();
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
            _mutexObj.ReleaseMutex();
        }

        public void DeleteAsync(int id)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == id);
            deserThirdPartyEvent.RemoveAt(thirdPartyEventIndex);
            _mutexObj.WaitOne();
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
            _mutexObj.ReleaseMutex();
        }

        public IEnumerable<ThirdPartyEvent> GetAllAsync()
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            return deserThirdPartyEvent;
        }

        public void UpdateAsync(ThirdPartyEvent thirdPartyEvent)
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