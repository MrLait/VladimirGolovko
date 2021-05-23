using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Models
{
    public class ThirdPartyEventRepository : IThirdPartyEventRepository
    {
        public void Create(ThirdPartyEvent thirdPartyEvent)
        {
#pragma warning disable S1075 // URIs should not be hardcoded
            var filePath = @"e:\VS\GitHub\.NET-GSTU-Winter-2021\VladimirGolovko\src\ThirdPartyEventEditor\ThirdPartyEventEditor\App_Data\ThirdPartyEvent.json";
#pragma warning restore S1075 // URIs should not be hardcoded

            var jsonData = System.IO.File.ReadAllText(filePath);
            var employeeList = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(jsonData) ?? new List<ThirdPartyEvent>();

            // Add any new employees
            employeeList.Add(thirdPartyEvent);
            jsonData = JsonConvert.SerializeObject(employeeList);
            System.IO.File.WriteAllText(filePath, jsonData);
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ThirdPartyEvent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(ThirdPartyEvent thirdPartyEvent)
        {
            throw new NotImplementedException();
        }
    }
}