using System;

namespace Order.Domain.Entities
{
    /// <summary>
    /// One line of an order: which product, and how many. A value object — the
    /// Order aggregate owns its lines and nothing else holds one directly.
    /// </summary>
    public class OrderLine
    {
        private OrderLine(Guid productId, int quantity)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("A line needs a product id", nameof(productId));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            ProductId = productId;
            Quantity = quantity;
        }

        public static OrderLine Of(Guid productId, int quantity)
        {
            return new OrderLine(productId, quantity);
        }

        public Guid ProductId { get; }
        public int Quantity { get; }
    }
}
