using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common.Entites;
using Order.Domain.Events;

namespace Order.Domain.Entities
{
    /// <summary>
    /// An order placed by one customer for one or more products. Framework-free,
    /// like Customer and Product: constructing one raises OrderCreated, and the
    /// Application layer decides whether the surrounding saga lets it persist.
    /// </summary>
    public class Order : AggregateRoot<Guid>
    {
        private readonly List<OrderLine> _lines;
        private readonly Guid _customerId;

        private Order(Guid customerId, IEnumerable<OrderLine> lines)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentNullAggregateException(nameof(customerId));

            var materialised = (lines ?? throw new ArgumentNullAggregateException(nameof(lines))).ToList();
            if (materialised.Count == 0)
                throw new ArgumentNullAggregateException(nameof(lines));

            Id = Guid.NewGuid();
            _customerId = customerId;
            _lines = materialised;

            AddDomainEvent(new OrderCreated(this));
        }

        public static Order Of(Guid customerId, IEnumerable<OrderLine> lines)
        {
            return new Order(customerId, lines);
        }

        public Guid CustomerId => _customerId;
        public IReadOnlyList<OrderLine> Lines => _lines;
    }
}
