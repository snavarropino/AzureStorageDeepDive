using Microsoft.Extensions.Configuration;

namespace CliUtils
{
    public class CommandArguments
    {
        public IConfigurationRoot Configuration { get; set; }
        private readonly string[] _args;
        public string[] Args => _args;
        public string Command { get; }
        public bool CommandHelpRequested => _args[0].IsHelp();

        public int LoopInterval { get; private set; }
        public bool LoopRequested { get; private set; }

        public CommandArguments(string command, string[] args)
        {
            _args = args;
            Command = command;
            BuildConfiguration();
        }

        public CommandArguments(string[] args)
        {
            _args = args;
            Command = string.Empty;
            BuildConfiguration();
        }

        private void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(_args);
            Configuration= builder.Build();

            var loop = Configuration["Loop"] ?? Configuration["l"];
            LoopRequested= loop != null;

            if (LoopRequested)
            {
                LoopInterval = int.Parse(loop);
            }
            else
            {
                LoopInterval = 0;
            }

        }
    }
}