using Autofac;
using Autofac.Integration.WebApi;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(Publisher.Startup))]
namespace Publisher
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            IContainer container = null;
            var builder = new ContainerBuilder();

            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    IRabbitMqHost rabbitMqHost = cfg.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), settings =>
                    {
                        settings.Username(ConfigurationManager.AppSettings["RabbitMQUser"]);
                        settings.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                    });
                });

                return busControl;
            })
            .As<IBusControl>()
            .As<IBus>()
            .SingleInstance();

            // Register Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Resolve dependencies
            container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            WebApiConfig.Register(config);
            SwaggerConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);

            // Register the Autofac middleware FIRST.
            app.UseAutofacMiddleware(container);
            app.UseWebApi(config);

            // Starts MassTransit Service bus, and registers stopping of bus on app dispose
            var bus = container.Resolve<IBusControl>();
            var busHandle = bus.StartAsync();
            var properties = new AppProperties(app.Properties);
            if (properties.OnAppDisposing != CancellationToken.None)
            {
                properties.OnAppDisposing.Register(() => busHandle.Result.StopAsync(TimeSpan.FromSeconds(30)));
            }
        }
    }
}
