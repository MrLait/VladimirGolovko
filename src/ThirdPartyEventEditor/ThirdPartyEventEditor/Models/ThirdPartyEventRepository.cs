using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Models
{
    public class ThirdPartyEventRepository : IThirdPartyEventRepository
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ThirdPartyEventJson"]);
        private readonly IJsonSerializer<ThirdPartyEvent> _jsonSerializer;

        public ThirdPartyEventRepository(IJsonSerializer<ThirdPartyEvent> jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public void Create(ThirdPartyEvent thirdPartyEvent)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            thirdPartyEvent.Id = deserThirdPartyEvent.LastOrDefault()?.Id + 1 ?? 1;
            deserThirdPartyEvent.Add(thirdPartyEvent);
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
        }

        public void Delete(int id)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == id);
            deserThirdPartyEvent.RemoveAt(thirdPartyEventIndex);
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
        }

        public async Task<IEnumerable<ThirdPartyEvent>> GetAllAsync()
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            for (int i = 0; i < deserThirdPartyEvent.Count; i++)
            {
                try
                {
                    deserThirdPartyEvent[i].PosterImage = await UploadSampleImage(deserThirdPartyEvent[i].PosterImage);
                }
                catch (FileNotFoundException)
                {
                    deserThirdPartyEvent[i].PosterImage = string.Empty;
                }
            }

            return deserThirdPartyEvent;
        }

        public void Update(ThirdPartyEvent thirdPartyEvent)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == thirdPartyEvent.Id);
            deserThirdPartyEvent[thirdPartyEventIndex] = thirdPartyEvent;
            _jsonSerializer.SerializeObjectsToJson(deserThirdPartyEvent, _filePath);
        }

        public ThirdPartyEvent GetById(int id)
        {
            var deserThirdPartyEvent = _jsonSerializer.DeserializeObjectsFromJson(_filePath).ToList();
            var thirdPartyEventIndex = deserThirdPartyEvent.FindIndex(x => x.Id == id);
            var thirdPartyEvent = deserThirdPartyEvent.ElementAt(thirdPartyEventIndex);
            return thirdPartyEvent;
        }

        private async Task<string> UploadSampleImage(string imageName)
        {
            var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data/"), imageName);
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}