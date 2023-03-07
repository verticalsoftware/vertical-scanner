using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;

namespace Vertical.Scanner.Output;

public class OutputControllerFactory : IOutputControllerFactory
{
    private readonly IOptions<ScanOptions> _scanOptions;
    private readonly IOutputWriter _writer;
    private readonly bool _writePaths;

    public OutputControllerFactory(
        IEnumerable<ISourceInput> sourceInputs,
        IOptions<ScanOptions> scanOptions,
        IOutputWriter writer)
    {
        _writePaths = !scanOptions.Value.NoPaths &&
                      sourceInputs.Count(input => input.Context == SourceInputContext.File) > 1;
        _scanOptions = scanOptions;
        _writer = writer;
    }

    /// <inheritdoc />
    public IOutputController CreateController(ISourceInput sourceInput)
    {
        switch (_scanOptions.Value)
        {
            case { PrintAll: true }:
                return new IndiscriminateOutputController(_scanOptions.Value, sourceInput, _writer, _writePaths);

            case { SurroundSpec: { } }:
                return new SurroundRegionOutputController(_scanOptions.Value, sourceInput, _writer, _writePaths);

            default:
                return new DefaultOutputController(_scanOptions.Value, sourceInput, _writer, _writePaths);
        }
    }
}