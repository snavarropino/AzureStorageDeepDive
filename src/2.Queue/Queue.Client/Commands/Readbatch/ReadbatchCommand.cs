using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;
using Queue.Cli.Commands.Dequeue;

namespace Queue.Cli.Commands.Readbatch
{
    public class ReadbatchCommand : ICommand
    {
        private IConfiguration Configuration { get; }

        public ReadbatchCommand(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                await ReadBatch.DeQueueMessageAsync(commandData);
            }
            else
            {
                PrintFullCommandHelp();
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

        public void PrintFullCommandHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help=
$@"Readbatch: read several messages in batch. Messages are hidden to other clients during 1 minute

    Usage: {executable} {nameof(ReadbatchCommand)}  --q=<queue> [--a=<account> --k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }

        public string GetShortCommandHelp()
        {
            return "Read several messages in batch";
        }
    }
}