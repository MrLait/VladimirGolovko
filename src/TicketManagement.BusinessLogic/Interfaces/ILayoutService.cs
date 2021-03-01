using System.Collections.Generic;
using TicketManagement.BusinessLogic.DTO;

namespace TicketManagement.BusinessLogic.Interfaces
{
    internal interface ILayoutService
    {
        void CreateLayout(LayoutDto layoutDto);

        LayoutDto GetLayout(int? id);

        IEnumerable<LayoutDto> GetLayouts();
    }
}
