using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class SentEmailEvent
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public DateTime EventTimestampe { get; set; }

        public object RawEventData { get; set; }
    }

}
