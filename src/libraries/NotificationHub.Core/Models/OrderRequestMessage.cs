using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Core.Models
{
    public class OrderRequestMessage
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string StoreId { get; set; }
        public string ProductId { get; set; }
        public string Instructions { get; set; }
    }
}
