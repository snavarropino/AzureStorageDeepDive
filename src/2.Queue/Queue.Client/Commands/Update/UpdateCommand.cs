using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;
using Queue.Cli.Commands.Dequeue;

namespace Queue.Cli.Commands.Update
{
    public class UpdateCommand : ICommand
    {
        private IConfiguration Configuration { get; }

        public UpdateCommand(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                await Update.UpdateMessageAsync(commandData);
            }
            else
            {
                PrintHelp();
            }
        }

        private UpdateCommandData ParseArgs()
        {

            return new UpdateCommandData()
            {
                UpdateMessage = Configuration["Message"] ?? Configuration["m"],
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

    Usage: {executable} {nameof(DequeueCommand)}  --q=<queue> [--a=<account> -k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }
    }
}