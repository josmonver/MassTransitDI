
namespace INCASOL.IP.Infrastructure.Events
{
    public interface IHandle<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}