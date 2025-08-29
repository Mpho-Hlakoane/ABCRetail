using Azure;
using Azure.Data.Tables;
using System;

namespace ABCRetailWebApp.Models
{
    public class AdminEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Admin";
        public string RowKey { get; set; } = string.Empty;  
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
