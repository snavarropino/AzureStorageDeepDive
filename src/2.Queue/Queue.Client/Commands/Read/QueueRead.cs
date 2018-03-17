using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Client.Commands
{
    internal static class QueueRead
    {
        public static async Task ReadMessageAsync(BaseCommandData insertCommandData)
        {
            var storageAccount = StorageAccountFactory.Get(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            // Peek
            var retrievedMessage = await queue.GetMessageAsync();
            Console.WriteLine($"Readed: {retrievedMessage.AsString}");
        }
    }
}