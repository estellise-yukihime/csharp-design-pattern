namespace pattern.chain_of_responsibility.normal;

public class Chain1 : IChainOfResponsibility
{
    private readonly IChainOfResponsibility? _next;

    public Chain1(IChainOfResponsibility next)
    {
        _next = next;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("chain: I am the chain number 1");
        
        if (_next is not null)
        {
            await _next.ExecuteAsync();
        }
    }
}