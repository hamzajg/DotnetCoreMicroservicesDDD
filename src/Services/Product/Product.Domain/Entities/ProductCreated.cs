using Domain.Common.Events;

namespace Product.Domain.Entities
{
    internal class ProductCreated : DomainEvent
    {
        private readonly Product _product;
        public Product Product => _product;

        public ProductCreated(Product product)
        {
            this._product = product;
        }
    }
}