using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Queue.Cli.Commands.Insert;

namespace Queue.Cli.Commands.ContextAndOptions
{
    internal class ContextAndOptions
    {
        public OperationContext Context { get; }
        public QueueRequestOptions Options { get; set; }

        public ContextAndOptions()
        {
            Context = new OperationContext()
            {
                ClientRequestID = $"my id {DateTime.UtcNow.Millisecond}",
                LogLevel = LogLevel.Verbose
            };

            Context.RequestCompleted += (o, requestEventArgs) =>
                {
                    Console.WriteLine($"Request to queue completed.Client Id='" +
                                      $"{(o as OperationContext).ClientRequestID}' " +
                                      $"ServerId='{requestEventArgs.RequestInformation.ServiceRequestID}'");
                };

            Options = new QueueRequestOptions()
            {
                LocationMode = LocationMode.PrimaryThenSecondary,
                RetryPolicy = new LinearRetry(),
                MaximumExecutionTime = TimeSpan.FromSeconds(5),
                ServerTimeout = TimeSpan.FromSeconds(2)
            };
        }

        public async Task InsertMessageAsync(InsertCommandData insertCommandData)
        {
            var storageAccount = GetStorageAccount(insertCommandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertCommandData.Queue);

            // Create the queue if it doesn't already exist.
            await queue.CreateIfNotExistsAsync(Options, Context);

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(insertCommandData.Message);
            await queue.AddMessageAsync(message);

            Console.WriteLine($"Inserted: {insertCommandData.Message}");
        }

        public async Task DeQueueMessageAsync(BaseCommandData commandData)
        {
            var storageAccount = StorageAccountFactory.Get(commandData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(commandData.Queue);

            // Read
            var retrievedMessage = await queue.GetMessageAsync(TimeSpan.FromSeconds(30),
                                                               Options,
                                                               Context);

            Console.WriteLine($"Readed: {retrievedMessage.AsString}");

            //Do stuff...

            await queue.DeleteMessageAsync(retrievedMessage, 
                                           new QueueRequestOptions(),
                                           Context);

            Console.WriteLine($"Deleted: {retrievedMessage.AsString}");
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