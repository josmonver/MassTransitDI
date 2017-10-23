using System;
using System.Linq;

namespace INCASOL.IP.Infrastructure.Events
{
    public interface IDomainEvent
    {
        DateTime DateTimeEventOccurred { get; }
    }
}
