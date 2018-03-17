using System;
using System.Reflection;
using System.Threading.Tasks;
using CliUtils;

namespace Queue.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Azure Storage Queue Client{System.Environment.NewLine}");
            var commandArguments = args.EnsureArguments();

            if (commandArguments != null)
            {
                var typeStr = $"Queue.Client.Commands.{commandArguments.Command.UppercaseFirst()}, Queue.Client";

                Object o = Activator.CreateInstance(Type.GetType(typeStr), new object[] { commandArguments.Args });
                await (o as ICommand).ExecuteAsync();
            }
            else
            {
                PrintHelp();
            }
        }

        private static void PrintHelp()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var name = System.AppDomain.CurrentDomain.FriendlyName;
            Console.WriteLine($@"Usage: {assemblyName} command <arguments>

Commands:

    Insert: Insert a new message in the queue. Type {assemblyName} insert -h for further details

");
        }
    }
}
