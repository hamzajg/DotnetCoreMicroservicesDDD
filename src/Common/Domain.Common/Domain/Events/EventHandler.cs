using System.Threading.Tasks;
namespace Domain.Common.Events
{
    public interface EventHandler<in TEvent> where TEvent : Event
    {
         Task HandleAsync(Event @event);
    }
}