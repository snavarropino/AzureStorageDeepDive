using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Cli.Commands.Update
{
    internal static class Update
    {
        public static async Task UpdateMessageAsync(UpdateCommandData updateCommandData)
        {
            var storageAccount = StorageAccountFactory.Get(updateCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(updateCommandData.Queue);

            // Get
            var retrievedMessage = await queue.GetMessageAsync();
            Console.WriteLine($"Readed: {retrievedMessage.AsString}");

            //Update with new text
            retrievedMessage.SetMessageContent(updateCommandData.UpdateMessage);
            await queue.UpdateMessageAsync(retrievedMessage,
                TimeSpan.FromSeconds(60.0),  // Make it invisible for another 60 seconds.
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);

            Console.WriteLine($"Updated: {retrievedMessage.AsString}");
        }
    }
}