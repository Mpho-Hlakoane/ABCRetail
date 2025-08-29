using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;

namespace ABCRetail.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var queueName = configuration["AzureStorage:QueueName"] ?? "orders";

            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<List<string>> PeekMessagesAsync(int maxMessages = 10)
        {
            var messages = new List<string>();
            var peeked = await _queueClient.PeekMessagesAsync(maxMessages);
            foreach (var msg in peeked.Value)
            {
                messages.Add(msg.MessageText);
            }
            return messages;
        }
    }
}
