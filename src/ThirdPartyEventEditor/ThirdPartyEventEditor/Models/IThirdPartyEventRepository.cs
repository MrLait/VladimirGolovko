using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Models
{
    public interface IThirdPartyEventRepository
    {
        void Create(ThirdPartyEvent thirdPartyEvent);

        void Update(ThirdPartyEvent thirdPartyEvent);

        Task<bool> Delete(string id);

        Task<IEnumerable<ThirdPartyEvent>> GetAll();
    }
}