using Domain.Common.Events;

namespace Product.Domain.Entities
{
    public class ProductTitleChanged : DomainEvent
    {
        private readonly Product _product;

        public ProductTitleChanged(Product product)
        {
            this.Version = ++Version;

            this._product = product;
        }
    }
}