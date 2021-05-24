using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Models
{
    public interface IThirdPartyEventRepository
    {
        Task CreateAsync(ThirdPartyEvent thirdPartyEvent);

        Task UpdateAsync(ThirdPartyEvent thirdPartyEvent);

        Task DeleteAsync(int id);

        Task<IEnumerable<ThirdPartyEvent>> GetAllAsync();

        ThirdPartyEvent GetById(int id);
    }
}