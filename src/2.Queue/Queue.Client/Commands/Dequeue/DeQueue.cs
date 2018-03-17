using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;

namespace Queue.Client.Commands
{
    public class Dequeue : ICommand
    {
        private IConfiguration Configuration { get; }

        public Dequeue(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                await QueueDequeue.DeQueueMessageAsync(commandData);
            }
            else
            {
                PrintHelp();
            }
        }

        private BaseCommandData ParseArgs()
        {

            return new BaseCommandData()
            {
                Queue = Configuration["Queue"]?? Configuration["q"],
                StorageAccount = Configuration["Account"] ?? Configuration["a"],
                StorageKey = Configuration["Key"] ?? Configuration["k"],
            };
        }

        public void PrintHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help=
$@"Peek: DeQueue first message in a queue. Message is hidden to other clients during 30 seconds

    Usage: {executable} {nameof(Dequeue)}  --q=<queue> [--a=<account> -k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }
    }
}