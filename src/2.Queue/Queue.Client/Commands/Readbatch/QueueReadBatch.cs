using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Queue.Client.Commands
{
    internal static class QueueReadBatch
    {
        public static async Task DeQueueMessageAsync(BaseCommandData insertCommandData)
        {
            var storageAccount = StorageAccountFactory.Get(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            
            var options = new QueueRequestOptions()
            {
                LocationMode = LocationMode.PrimaryThenSecondary,
                RetryPolicy = new LinearRetry(),
                MaximumExecutionTime = TimeSpan.FromSeconds(2),
                ServerTimeout = TimeSpan.FromSeconds(3)
            };

            // Read batch
            var readed = await queue.GetMessagesAsync(5, TimeSpan.FromMinutes(5), options, new OperationContext());

            foreach (CloudQueueMessage retrievedMessage in readed)
            {
                Console.WriteLine($"Readed: {retrievedMessage.AsString}");
                // Process all messages in less than 5 minutes, deleting each message after processing.
                await queue.DeleteMessageAsync(retrievedMessage);
                //Do stuff...
                Console.WriteLine($"Deleted: {retrievedMessage.AsString}");
            }
        }
    }
}