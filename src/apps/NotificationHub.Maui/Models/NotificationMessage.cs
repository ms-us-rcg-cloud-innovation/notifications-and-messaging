using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.Models;

public class NotificationMessage
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime TimeStamp { get; set; }
    public IDictionary<string, string> Tags { get; set; }
    public IDictionary<string, object> Data { get; set; }

}


