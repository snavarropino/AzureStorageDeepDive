using System.Threading.Tasks;

namespace CliUtils
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}