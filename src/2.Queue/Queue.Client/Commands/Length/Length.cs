using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Cli.Commands.Length
{
    internal static class Length
    {
        public static async Task CountMessagesAsync(BaseCommandData insertCommandData)
        {
            var storageAccount = StorageAccountFactory.Get(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            
            // Fetch
            await queue.FetchAttributesAsync();

            // Display number of messages.
            Console.WriteLine($"Number of messages in queue: {queue.ApproximateMessageCount}");
        }
    }
}