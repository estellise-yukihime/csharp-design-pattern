namespace pattern.chain_of_responsibility.net_di;

public class ChainBuilder<TChain> where TChain : class
{
    private readonly List<Type> _handlers = [];
    private readonly IServiceCollection _services;
    private readonly ServiceLifetime _lifetime;

    public ChainBuilder(IServiceCollection services) : this(services, ServiceLifetime.Transient)
    {
    }

    public ChainBuilder(IServiceCollection serviceCollection, ServiceLifetime lifetime)
    {
        _services = serviceCollection;
        _lifetime = lifetime;
    }

    public ChainBuilder<TChain> AddHandler<THandler>()
        where THandler : class, TChain
    {
        _handlers.Add(typeof(THandler));

        return this;
    }

    public IServiceCollection Build()
    {
        _services.Add(new ServiceDescriptor(typeof(TChain), provider => InternalBuild(provider), _lifetime));

        return _services;
    }

    private TChain InternalBuild(IServiceProvider provider, int index = 0)
    {
        var cons = _handlers[index].GetConstructors();

        if (cons.Length is 0 or > 1)
        {
            throw new InvalidOperationException(
                $"Handler {_handlers[index].FullName} has multiple constructors or no public constructor");
        }

        var once = cons[0];
        var farm = once.GetParameters();

        if (farm.Any(x => x.ParameterType == typeof(TChain)) is false)
        {
            throw new InvalidOperationException(
                $"Handler {_handlers[index].FullName} does not have a constructor with TChain parameter");
        }

        var inst = new object[farm.Length];

        for (var i = 0; i < farm.Length; i++)
        {
            var type = farm[i].ParameterType;

            if (type == typeof(TChain))
            {
                if (index == _handlers.Count - 1)
                {
                    inst[i] = null!;
                }
                else
                {
                    inst[i] = InternalBuild(provider, index + 1);
                }
            }
            else
            {
                inst[i] = provider.GetRequiredService(type);
            }
        }

        return (TChain)Activator.CreateInstance(_handlers[index], inst)!;
    }
}