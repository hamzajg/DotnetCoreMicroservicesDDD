using Domain.Common.Events;

namespace Order.Domain.Events
{
    /// <summary>
    /// Raised when an Order is constructed. The Id mirrors the order's id so a
    /// publisher keyed by event id records one event per order.
    /// </summary>
    public class OrderCreated : DomainEvent
    {
        private readonly Entities.Order _order;
        public Entities.Order Order => _order;

        public OrderCreated(Entities.Order order)
        {
            _order = order;
            Id = order.Id;
        }
    }
}
