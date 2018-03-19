using System;
using System.Linq;
using System.Reflection;

namespace CliUtils
{
    public static class Help
    {
        public static void PrintGeneralHelp()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine($@"Usage: {assemblyName} command <arguments>

Commands:");

            var commands = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var commandType in commands)
            {
                if (!commandType.IsAbstract)
                {
                    var command = Activator.CreateInstance(commandType, new object[] { new string[] { } });
                    Console.WriteLine($"{commandType.Name.Replace("Command","")}: {(command as ICommand).GetShortCommandHelp()}");
                }
            }

            Console.WriteLine($@"
Arguments (general):

    --l=miliseconds: Execute an infinite loop, with a delay between each command execution

");
        }
    }
}