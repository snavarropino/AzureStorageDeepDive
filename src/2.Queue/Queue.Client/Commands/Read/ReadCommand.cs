using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;
using Queue.Cli.Commands.Dequeue;

namespace Queue.Cli.Commands.Read
{
    public class ReadCommand : ICommand
    {
        private IConfiguration Configuration { get; }

        public ReadCommand(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                await Read.ReadMessageAsync(commandData);
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
$@"Read: Read first message in a queue. Message is hidden to other clients during 30 seconds

    Usage: {executable} {nameof(ReadCommand)}  --q=<queue> [--a=<account> --k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }

        public string GetShortCommandHelp()
        {
            return "Read first message in a queue and hidden it for 30 seconds";
        }
    }
}