using Common;
using INCASOL.IP.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MyEventHandler : IHandle<MyEvent>
    {
        public void Handle(MyEvent args)
        {
            string s = "test";
            s = s.ToUpper();
        }
    }
}
