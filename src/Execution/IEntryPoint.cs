namespace Vertical.Scanner.Execution;

public interface IEntryPoint
{
    bool Handles();
    
    void Execute();
}