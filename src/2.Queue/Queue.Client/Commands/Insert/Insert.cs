using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Cli.Commands.Insert
{
    internal static class Insert
    {
        public static async Task InsertMessageAsync(InsertCommandData insertCommandData)
        {
            var storageAccount = GetStorageAccount(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            // Create the queue if it doesn't already exist.
            await queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(insertCommandData.Message);
            await queue.AddMessageAsync(message);

            Console.WriteLine($"Inserted: {insertCommandData.Message}");
        }

        private static CloudStorageAccount GetStorageAccount(InsertCommandData insertCommandData)
        {
            if (string.IsNullOrWhiteSpace(insertCommandData.StorageAccount))
            {
                return CloudStorageAccount.DevelopmentStorageAccount;
            }

            StorageCredentials storageCredentials = new StorageCredentials(insertCommandData.StorageAccount, insertCommandData.StorageKey);
            return new CloudStorageAccount(storageCredentials, useHttps: true);
        }
    }
}