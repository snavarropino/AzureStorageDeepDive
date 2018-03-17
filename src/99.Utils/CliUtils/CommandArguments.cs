using Microsoft.Extensions.Configuration;

namespace CliUtils
{
    public class CommandArguments
    {
        public string Command { get; }
        public string[] Args { get; }

        public CommandArguments(string command, string[] args)
        {
            Command = command;
            Args = args;
        }

        public CommandArguments(string[] args)
        {
            Command = string.Empty;
            Args = args;
        }
        
        public IConfigurationRoot AsConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(Args);
            return builder.Build();
        }
    }
}