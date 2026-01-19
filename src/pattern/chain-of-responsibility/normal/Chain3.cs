namespace pattern.chain_of_responsibility.normal;

public class Chain3 : IChainOfResponsibility
{
    private readonly IChainOfResponsibility? _next;

    public Chain3(IChainOfResponsibility next)
    {
        _next = next;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("chain: I am the chain number 3");
        
        if (_next is not null)
        {
            await _next.ExecuteAsync();
        }
    }
}