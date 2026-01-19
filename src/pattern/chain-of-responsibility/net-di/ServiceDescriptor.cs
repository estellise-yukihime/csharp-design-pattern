namespace pattern.chain_of_responsibility.net_di;

public class ServiceDescriptor(Type type, Func<IServiceProvider, object> func, ServiceLifetime lifetime)
{
}