﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CliUtils;

namespace File.Cli
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Azure Storage File Client{Environment.NewLine}");

            var commandArguments = args.BuildCommandArguments();

            if (commandArguments != null)
            {
                await ExecuteCommand(commandArguments);
            }
            else
            {
                Help.PrintGeneralHelp();
            }
        }

        private static async Task ExecuteCommand(CommandArguments commandArguments)
        {
            var command = GetCommand(commandArguments);

            if (commandArguments.CommandHelpRequested)
            {
                command.PrintFullCommandHelp();
            }
            else
            {
                if (commandArguments.LoopRequested)
                {
                    await ExecuteLoop(command, commandArguments.LoopInterval);
                }

                await command.ExecuteAsync(9);
            }
        }

        private static async Task ExecuteLoop(ICommand command, int interval)
        {
            int i = 0;

            while (true)
            {
                await command.ExecuteAsync(i++);
                Thread.Sleep(interval);
            }
        }

        private static ICommand GetCommand(CommandArguments commandArguments)
        {
            var commandName = commandArguments.Command.UppercaseFirst();
            var commandtype = $"File.Cli.Commands.{commandName}.{commandName}Command, File.Cli";

            var command = Activator.CreateInstance(Type.GetType(commandtype), new object[] {commandArguments.Args});
            return command as ICommand;
        }
    }
}