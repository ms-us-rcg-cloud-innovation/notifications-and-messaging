using Azure;

namespace Functions.Models
{
    public class AcsEmailTableEntity
        : Azure.Data.Tables.ITableEntity
    {
        public string Importance { get; set; }
        public string Subject { get; set; }
        public string Data { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
