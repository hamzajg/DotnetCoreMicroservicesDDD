using System;

namespace Services.Common.Domain
{
    public abstract class DomainEvent
    {
        public string Id {get; protected set;}
        public DateTime OcurrendOn {get;}

        public DomainEvent() {
            OcurrendOn = DateTime.UtcNow;
        }
    }
}