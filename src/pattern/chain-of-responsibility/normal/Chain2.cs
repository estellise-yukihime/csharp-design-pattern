namespace pattern.chain_of_responsibility.normal;

public class Chain2 : IChainOfResponsibility
{
    private readonly IChainOfResponsibility? _next;

    public Chain2(IChainOfResponsibility next)
    {
        _next = next;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("chain: I am the chain number 2");
        
        if (_next is not null)
        {
            await _next.ExecuteAsync();
        }
    }
}