using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Cli.Commands.Read
{
    internal static class Read
    {
        public static async Task ReadMessageAsync(BaseCommandData commandData)
        {
            var storageAccount = StorageAccountFactory.Get(commandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(commandData.Queue);

            // Read (it will make themessage invisible toother for 30 seconds)
            // If message is deleted it will come back visible (as is it would be queued again). 
            // In this case message DequeueCount property is inreased by 1
            var retrievedMessage = await queue.GetMessageAsync();
            if (retrievedMessage != null)
            {
                Console.WriteLine($"Readed: {retrievedMessage.AsString}");
            }
            else
            {
                Console.WriteLine("No messages on queue");
            }
        }
    }
}