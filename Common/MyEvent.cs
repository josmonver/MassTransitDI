using INCASOL.IP.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MyEvent : IDomainEvent
    {
        public DateTime DateTimeEventOccurred { get; set; }

        public MyEvent()
        {
            DateTimeEventOccurred = DateTime.Now;
        }
    }
}
