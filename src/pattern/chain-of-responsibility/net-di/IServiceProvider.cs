namespace pattern.chain_of_responsibility.net_di;

public interface IServiceProvider
{
    object GetRequiredService(Type type);
}