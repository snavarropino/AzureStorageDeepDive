using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Queue.Client.Commands
{
    public class Insert : ICommand
    {
        public object[] Args { get; }

        public Insert(object[] args)
        {
            Args = args;
        }

        public async Task ExecuteAsync()
        {
            if (!Args.Any() || (Args.Any() && ((string) Args.First()).IsHelp()))
            {
                PrintHelp();
                return;
            }

            var data=ParseArgs();
            if (data.Validate())
            {
                await InsertMessageAsync(data);
            }
            else
            {
                PrintHelp();
            }
        }

        private async Task InsertMessageAsync(InsertData insertData)
        {
            var storageAccount = GetStorageAccount(insertData);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(insertData.Queue);

            // Create the queue if it doesn't already exist.
            await queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(insertData.Message);
            await queue.AddMessageAsync(message);
        }

        private static CloudStorageAccount GetStorageAccount(InsertData insertData)
        {
            if (string.IsNullOrWhiteSpace(insertData.StorageAccount))
            {
                return CloudStorageAccount.DevelopmentStorageAccount;
            }

            StorageCredentials storageCredentials = new StorageCredentials(insertData.StorageAccount, insertData.StorageKey);
            return new CloudStorageAccount(storageCredentials, useHttps: true);
        }

        private InsertData ParseArgs()
        {
            var configuration = new CommandArguments(Args.Cast<string>().ToArray())
                    .AsConfiguration();

            return new InsertData()
            {
                Queue = configuration["Queue"]?? configuration["q"],
                Message = configuration["Message"] ?? configuration["m"],
                StorageAccount = configuration["Account"] ?? configuration["a"],
                StorageKey = configuration["Key"] ?? configuration["k"],
            };
        }

        private void PrintHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help=
$@"Insert: Insert a message in a queue. 

    Usage: {executable} {nameof(Insert)} --m='<mesage>' --q=<queue> [--a=<account> -k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }
    }
}