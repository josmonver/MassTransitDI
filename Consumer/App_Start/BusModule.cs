using Autofac;
using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Configuration;

namespace Consumer
{
    public class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    IRabbitMqHost rabbitMqHost = cfg.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), settings =>
                    {
                        settings.Username(ConfigurationManager.AppSettings["RabbitMQUser"]);
                        settings.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                    });
                    cfg.ReceiveEndpoint(rabbitMqHost, "IP.AgilePoint.queue", ec =>
                    {
                        ec.LoadFrom(context);
                    });
                });

                return busControl;
            })
            .SingleInstance()
            .As<IBusControl>()
            .As<IBus>();
        }
    }
}