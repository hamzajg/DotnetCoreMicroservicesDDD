using System.Threading.Tasks;
using Services.Common.Domain.Events;

namespace Customer.App.Services
{
    public interface IPublisherServices
    {

        Task PublishAsync(DomainEvent @event);
    }
}