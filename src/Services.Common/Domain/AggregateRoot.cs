using System;
using System.Collections.Generic;
using  Services.Common.Domain.Events;
namespace Services.Common.Domain
{
    public abstract class AggregateRoot<T> : Entity<T>
    {

        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

        private int _version = 0;
        public int Version => _version;

        protected AggregateRoot() 
        {

        }

        protected void AddDomainEvent(DomainEvent newEvent) 
        {
            ValidateVersion(newEvent.Version);
            newEvent.Version = ++_version;
            _domainEvents.Add(newEvent);
        }

        private void ValidateVersion(int version) 
        {
            if(Version != version) {
                throw new InvalidVersionAggregateException("Invalid version specified");
            }
        }
        

        private class InvalidVersionAggregateException : Exception 
        {
            public InvalidVersionAggregateException(string str) : base (str) {

            }
        }
        public class ArgumentNullAggregateException : AggregateException
        {
            public ArgumentNullAggregateException(string message) : base(message)
            {
            }
        }
    }
}