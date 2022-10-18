using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.MessagingFunctions.Models
{
    public class BroadcastMessageDistribution
    {
        public BroadcastMessageDistribution AndroidBroadcast { get; set; }

        public BroadcastMessageDistribution AppleBroadcast { get; set; }
    }
}
