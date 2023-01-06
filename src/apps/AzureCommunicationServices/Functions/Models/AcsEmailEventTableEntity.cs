using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class AcsEmailEventTableEntity
        : Azure.Data.Tables.ITableEntity
    {
        public string EngagementContext { get; set; }
        public string Sender { get; set; }
        public string UserAgent { get; set; }
        public string EngagementType { get; set; }
        public DateTime UserActionTimeStamp { get; set; }
        public string Data { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
