using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Cli.Commands.Dequeue
{
    internal static class Dequeue
    {
        public static async Task DeQueueMessageAsync(BaseCommandData insertCommandData)
        {
            var storageAccount = StorageAccountFactory.Get(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            // Read
            var retrievedMessage = await queue.GetMessageAsync();
            Console.WriteLine($"Readed: {retrievedMessage.AsString}");
            
            //Do stuff...

            await queue.DeleteMessageAsync(retrievedMessage);
            Console.WriteLine($"Deleted: {retrievedMessage.AsString}");
        }
    }
}