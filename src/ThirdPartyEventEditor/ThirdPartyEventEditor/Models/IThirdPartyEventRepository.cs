using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Models
{
    public interface IThirdPartyEventRepository
    {
        void CreateAsync(ThirdPartyEvent thirdPartyEvent);

        void UpdateAsync(ThirdPartyEvent thirdPartyEvent);

        void DeleteAsync(int id);

        IEnumerable<ThirdPartyEvent> GetAllAsync();

        ThirdPartyEvent GetById(int id);
    }
}