using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Queue.Cli.Commands.Readbatch
{
    internal static class ReadBatch
    {
        public static async Task DeQueueMessageAsync(BaseCommandData commandData)
        {
            var storageAccount = StorageAccountFactory.Get(commandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(commandData.Queue);

            // Read batch
            var readed = await queue.GetMessagesAsync(5, TimeSpan.FromMinutes(1), new QueueRequestOptions(), new OperationContext());

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