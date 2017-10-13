using MassTransit;
using Messaging;
using System.Web.Http;

namespace Publisher.Controllers
{
    public class PublishController : ApiController
    {
        IBusControl _bus;

        public PublishController(IBusControl bus)
        {
            _bus = bus;
        }

        public IHttpActionResult Post()
        {
            var obtencio = _bus.Publish<IFooMessage>(new
            {
                Foo = "Foo"
            });

            return Ok();
        }
    }
}