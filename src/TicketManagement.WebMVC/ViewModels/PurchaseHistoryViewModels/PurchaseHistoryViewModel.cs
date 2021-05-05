using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels
{
    public class PurchaseHistoryViewModel
    {
        public List<PurchaseHistoryItem> Items { get; init; } = new List<PurchaseHistoryItem>();

        public string UserId { get; set; }
    }
}
