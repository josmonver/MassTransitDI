using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consumer
{
    public class FooService : IFooService
    {
        public void DoStuff(string s)
        {

        }
    }

    public interface IFooService
    {
        void DoStuff(string s);
    }
}