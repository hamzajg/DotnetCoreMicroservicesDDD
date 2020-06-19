using Domain.Common.Entites;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Product.Domain.Entities
{
    public class Product : AggregateRoot<Guid>
    {

        private string _title;
        private float _oldPrice;
        private float _currentPrice;
        private Product(string title, float oldPrice, float currentPrice)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            this._title = title;
            this._oldPrice = oldPrice;
            this._currentPrice = currentPrice;

            AddDomainEvent(new ProductCreated(this));
        }

        public static Product Of(string title, float oldPrice, float currentPrice)
        {
            return new Product(title, oldPrice, currentPrice);
        }

        public string Title { get => _title; set => _title = value; }
        public float CurrentPrice { get => _currentPrice; set => _currentPrice = value; }
        public float OldPrice { get => _oldPrice; set => _oldPrice = value; }

        public void ChangeTitle(string newTitle)
        {
            this._title = newTitle;
            AddDomainEvent(new ProductTitleChanged(this));
        }

        public void ChangeCurrentPrice(float newCurrentPrice)
        {
            this._currentPrice = newCurrentPrice;
            AddDomainEvent(new ProductCurrentPriceChanged(this));
        }

        public void ChangeOldPrice(float newOldPrice)
        {
            this._oldPrice = newOldPrice;
            AddDomainEvent(new ProductOldPriceChanged(this));
        }
    }
}
