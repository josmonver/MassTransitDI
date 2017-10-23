using System;
using System.Linq;

namespace INCASOL.IP.Infrastructure.Events
{
    public interface IDomainEvents
    {
        void Register<T>(Action<T> callback) where T : IDomainEvent;
        void ClearCallbacks();
        void Raise<T>(T args) where T : IDomainEvent;
    }
}
