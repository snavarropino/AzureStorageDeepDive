using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;

namespace Queue.Client.Commands
{
    public class Insert : ICommand
    {
        private IConfiguration Configuration { get; }

        public Insert(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                commandData.Message = $"{commandData.Message} {i}";
                await QueueInsert.InsertMessageAsync(commandData);
                Console.WriteLine($"Inserted: {commandData.Message}");
            }
            else
            {
                PrintHelp();
            }
        }

        private InsertCommandData ParseArgs()
        {

            return new InsertCommandData()
            {
                Queue = Configuration["Queue"]?? Configuration["q"],
                Message = Configuration["Message"] ?? Configuration["m"],
                StorageAccount = Configuration["Account"] ?? Configuration["a"],
                StorageKey = Configuration["Key"] ?? Configuration["k"],
            };
        }

        public void PrintHelp()
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