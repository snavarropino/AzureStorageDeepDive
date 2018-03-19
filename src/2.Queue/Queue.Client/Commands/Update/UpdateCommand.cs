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
                PrintFullCommandHelp();
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

        public void PrintFullCommandHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help=
$@"Update: Update first message in a queue. Text is updated and message is hidden to other clients during 60 seconds

    Usage: {executable} {nameof(UpdateCommand)}  --q=<queue> --m=<message> [--a=<account> --k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }

        public string GetShortCommandHelp()
        {
            return "Update first message in a queue";
        }
    }
}