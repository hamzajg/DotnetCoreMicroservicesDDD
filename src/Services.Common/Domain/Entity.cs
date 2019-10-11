using System.Collections.Generic;
using System;
namespace Services.Common.Domain
{
    public abstract class Entity<T> : IEquatable<Entity<T>>
    {
        public T Id {get; protected set;}
        protected Entity()
        {            
        }

        protected Entity(T id) 
        {
            Id = id;
        }

        public override bool Equals(object obj) 
        {
            return Equals(obj as Entity<T>);
        }

        public bool Equals(Entity<T> other)
        {
            return other != null && EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(Entity<T> entity1, Entity<T> entity2) => EqualityComparer<Entity<T>>.Default.Equals(entity1, entity2);
        
        public static bool operator !=(Entity<T> entity1, Entity<T> entity2) => (entity1 == entity2);
        
    }
}