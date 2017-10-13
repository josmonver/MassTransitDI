using Autofac;

namespace Consumer
{
    public class ConsumersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FooConsumer>();
        }
    }
}