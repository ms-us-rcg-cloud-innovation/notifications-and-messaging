using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Core.Models
{
    public abstract class NotificationMessageBase
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
