using System.Threading.Tasks;

namespace CliUtils
{
    public interface ICommand
    {
        Task ExecuteAsync(int i);
        void PrintHelp();
    }
}