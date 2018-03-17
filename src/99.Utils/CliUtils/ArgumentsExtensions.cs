using System;
using System.Linq;

namespace CliUtils
{
    public static class ArgumentsExtensions
    {
        public static bool IsHelp(this string str)
        {
            return str.Equals("help", StringComparison.InvariantCultureIgnoreCase)
                    || str.Equals("-h", StringComparison.InvariantCultureIgnoreCase)
                   ||  str.Equals("--h", StringComparison.InvariantCultureIgnoreCase)
                    || str.Equals("/?", StringComparison.InvariantCultureIgnoreCase);
        }

        public static CommandArguments BuildCommandArguments(this string[] args)
        {
            if (args.Length < 1)
            {
                return null;
            }

            return new CommandArguments(args[0], args.Skip(1).ToArray());
        }

        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}