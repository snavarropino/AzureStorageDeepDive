using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Cli.Commands.Peek
{
    internal static class Peek
    {
        public static async Task PeekMessageAsync(BaseCommandData commandData)
        {
            var storageAccount = StorageAccountFactory.Get(commandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(commandData.Queue);

            // Peek
            var peekedMessage = await queue.PeekMessageAsync();
            Console.WriteLine(peekedMessage.AsString);
        }
    }
}