using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;
using Vertical.Scanner.Output;

namespace Vertical.Scanner.Scanning;

public class FileCountScanHandler : IScanHandler
{
    private readonly IOutputWriter _outputWriter;
    private readonly Lazy<ILineMatcher> _lazyLineMatcher;
    private int _internalCount;

    public FileCountScanHandler(ILineMatcherFactory lineMatcherFactory, IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
        _lazyLineMatcher = new Lazy<ILineMatcher>(lineMatcherFactory.CreateLineMatcher);
    }
    
    /// <inheritdoc />
    public void SourceScanStarting()
    {
    }

    /// <inheritdoc />
    public void SourceScanCompleted()
    {
        _outputWriter.WriteFileInfo($"Files matched: {_internalCount}");
    }

    /// <inheritdoc />
    public void ScanSource(ISourceInput sourceInput)
    {
        var matcher = _lazyLineMatcher.Value;

        while (sourceInput.TryReadNext(out var content))
        {
            if (!matcher.IsMatch(content)) continue;
            
            ++_internalCount;
            return;
        }
    }
}