using System;
using Xunit;
using System.Threading.Tasks;
using Entities = Product.Domain.Entities;

namespace Tests.Product.Domain
{
    public class ProductTest : IDisposable
    {
        private static Entities.Product _product;
        public ProductTest()
        {
            _product = Entities.Product.Of("Test Product", 50F, 30F);
        }
        public void Dispose()
        {

        }

        [Fact]
        public void NewProductWithEmptyTitleMustThrowsException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.Run(() => Entities.Product.Of("", 0F, 0F)));
        }
        
        [Fact]
        public void CanCreateNewProduct()
        {
            Assert.NotNull(_product);
            Assert.Equal("Test Product", _product.Title);
        }

        [Fact]
        public void CanChangeProductTitle()
        {
            _product.ChangeTitle("Change Tilte");

            Assert.NotNull(_product);
            Assert.Equal("Change Tilte", _product.Title);
        }

        [Fact]
        public void CanChangeProductOldPrice()
        {
            _product.ChangeOldPrice(50F);

            Assert.NotNull(_product);
            Assert.Equal(50F, _product.OldPrice);
        }

        [Fact]
        public void CanChangeProductCurrentPrice()
        {
            _product.ChangeCurrentPrice(50F);

            Assert.NotNull(_product);
            Assert.Equal(50F, _product.CurrentPrice);
        }
    }
}
