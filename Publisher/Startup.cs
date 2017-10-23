using Autofac;
using Autofac.Integration.WebApi;
using INCASOL.IP.Infrastructure.Events;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.Cors;
using Owin;
using Services;
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
            AutofacWebApiDependencyResolver dependencyResolver = null;
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

            builder.RegisterType<MyService>().As<IMyService>().InstancePerRequest();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AsClosedTypesOf(typeof(IHandle<>))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // builder.Register(c => new QueryProcessor(c.Resolve<IComponentContext>()))
            //builder.Register<IDomainEvents>(_ => new DomainEvents(container, "")).InstancePerRequest();
            //builder.Register<IDomainEvents>(_ => new DomainEvents(container)).InstancePerRequest();
            builder.RegisterType<DomainEvents>().As<IDomainEvents>().InstancePerRequest();

            // Register Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Resolve dependencies
            container = builder.Build();
            dependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = dependencyResolver;

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
