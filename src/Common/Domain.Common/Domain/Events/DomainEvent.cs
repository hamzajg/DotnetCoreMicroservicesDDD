using System;

namespace Domain.Common.Events
{
    public abstract class DomainEvent : Event
    {
        public Guid Id {get; protected set;}
        public DateTime OcurrendOn {get;}
        public int Version { get; set; }//internal set; }

        public DomainEvent() {
            OcurrendOn = DateTime.UtcNow;
        }
    }
}