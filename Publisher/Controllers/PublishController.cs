using MassTransit;
using Messaging;
using Services;
using System.Web.Http;

namespace Publisher.Controllers
{
    public class PublishController : ApiController
    {
        //IBusControl _bus;

        //public PublishController(IBusControl bus)
        //{
        //    _bus = bus;
        //}

        //public IHttpActionResult Post()
        //{
        //    var obtencio = _bus.Publish<IFooMessage>(new
        //    {
        //        Foo = "Foo"
        //    });

        //    return Ok();
        //}

        IMyService _service;

        public PublishController(IMyService service)
        {
            _service = service;
        }

        public IHttpActionResult Post()
        {
            _service.Do();

            return Ok();
        }
    }
}