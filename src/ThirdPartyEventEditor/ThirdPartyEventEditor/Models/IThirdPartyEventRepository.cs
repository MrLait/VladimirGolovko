using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ClassicMvc.Services
{
    public interface IThirdPartyEventRepository
    {
        void Create(ThirdPartyEvent thirdPartyEvent);

        void Update(ThirdPartyEvent thirdPartyEvent);

        void Delete(int id);

        IEnumerable<ThirdPartyEvent> GetAll();

        ThirdPartyEvent GetById(int id);
    }
}