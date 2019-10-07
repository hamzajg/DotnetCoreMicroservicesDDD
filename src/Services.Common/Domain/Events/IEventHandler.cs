using System.Threading.Tasks;
namespace Services.Common.Domain.Events
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
         Task HandleAsync(IEvent @event);
    }
}