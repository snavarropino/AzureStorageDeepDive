using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;

namespace Queue.Cli.Commands.Insert
{
    public class InsertCommand : ICommand
    {
        private IConfiguration Configuration { get; }

        public InsertCommand(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                commandData.Message = $"{commandData.Message} {i}";
                await Insert.InsertMessageAsync(commandData);
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

    Usage: {executable} {nameof(InsertCommand)} --m='<mesage>' --q=<queue> [--a=<account> -k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }
    }
}