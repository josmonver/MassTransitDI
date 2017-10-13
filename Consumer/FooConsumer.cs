using MassTransit;
using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Consumer
{
    public class FooConsumer : IConsumer<IFooMessage>
    {
        private IFooService _service;

        public FooConsumer(IFooService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<IFooMessage> context)
        {
            IFooMessage @event = context.Message;

            _service.DoStuff(@event.Foo);

            return Task.FromResult(context.Message);
        }
    }
}