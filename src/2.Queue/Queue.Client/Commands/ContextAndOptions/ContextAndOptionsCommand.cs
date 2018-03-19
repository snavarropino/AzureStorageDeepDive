using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CliUtils;
using Microsoft.Extensions.Configuration;
using Queue.Cli.Commands.Insert;

namespace Queue.Cli.Commands.ContextAndOptions
{
    public class ContextAndOptionsCommand : ICommand
    {
        private IConfiguration Configuration { get; }

        public ContextAndOptionsCommand(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData=ParseArgs();
            if (commandData.Validate())
            {
                var index = (i > 0) ? i.ToString() : string.Empty;
                commandData.Message = $"{commandData.Message} {index}";
                var contextAndOptions = new ContextAndOptions();
                await contextAndOptions.InsertMessageAsync(commandData);
                Thread.Sleep(2000);
                await contextAndOptions.DeQueueMessageAsync(commandData);

                Console.WriteLine($"Context: {contextAndOptions.Context.ClientRequestID}, StartTime: {contextAndOptions.Context.StartTime}, {contextAndOptions.Context.EndTime}");
            }
            else
            {
                PrintFullCommandHelp();
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

        public void PrintFullCommandHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help=
$@"ContextAndOptions: Insert a message in a queue and dequeue it, using options and an operation context. 

    Usage: {executable} {nameof(ContextAndOptions)} --m='<mesage>' --q=<queue> [--a=<account> -k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";
            
            Console.WriteLine(help);
        }

        public string GetShortCommandHelp()
        {
            return "Insert a message in a queue and dequeue it, using options and an operation context";
        }
    }
}