namespace Services.Common.Domain
{
    public abstract class AggregateRoot<T> : Entity<T>
    {
        protected AggregateRoot() {
            
        }
        
    }
}