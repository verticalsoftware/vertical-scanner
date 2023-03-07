using Vertical.Scanner.Input;

namespace Vertical.Scanner.Output;

public interface IOutputControllerFactory
{
    IOutputController CreateController(ISourceInput sourceInput);
}