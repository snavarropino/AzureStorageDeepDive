using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Client.Commands
{
    internal static class QueuePeek
    {
        public static async Task PeekMessageAsync(BaseCommandData insertCommandData)
        {
            var storageAccount = StorageAccountFactory.Get(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            // Peek
            var peekedMessage = await queue.PeekMessageAsync();
            Console.WriteLine(peekedMessage.AsString);
        }
    }
}