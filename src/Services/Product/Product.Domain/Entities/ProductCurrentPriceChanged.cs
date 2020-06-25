using Domain.Common.Events;

namespace Product.Domain.Entities
{
    internal class ProductCurrentPriceChanged : DomainEvent
    {
        private Product product;

        public ProductCurrentPriceChanged(Product product)
        {
            this.Version = ++Version;

            this.product = product;
        }
    }
}