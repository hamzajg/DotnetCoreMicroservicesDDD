using Domain.Common.Events;

namespace Product.Domain.Entities
{
    internal class ProductOldPriceChanged : DomainEvent
    {
        private Product product;

        public ProductOldPriceChanged(Product product)
        {
            this.Version += Version;
            this.product = product;
        }
    }
}