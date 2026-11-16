using System;
using System.Collections.Generic;
using Domain.Common.Commands;

namespace Order.Application.Commands
{
    /// <summary>
    /// Places an order for one customer over one or more products. The caller
    /// does not supply an order id — the Order aggregate generates one.
    /// </summary>
    public class CreateOrderCommand : Command
    {
        public Guid CustomerId { get; set; }
        public List<CreateOrderItem> Items { get; set; } = new List<CreateOrderItem>();
    }

    public class CreateOrderItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
