using System.Threading.Tasks;
namespace Domain.Common.Commands
{
    public interface CommandHandler<in TCommand> where TCommand : Command
    {

        Task HandleAsync(TCommand command);
         
    }
}