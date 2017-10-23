using Common;
using INCASOL.IP.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MyService : IMyService
    {
        IDomainEvents _domainEvents;

        public MyService(IDomainEvents domainEvents)
        {
            _domainEvents = domainEvents;
        }

        public void Do()
        {
            _domainEvents.Raise<MyEvent>(new MyEvent());
        }
    }

    public interface IMyService
    {
        void Do();
    }
}
