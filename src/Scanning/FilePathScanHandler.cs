using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;
using Vertical.Scanner.Output;

namespace Vertical.Scanner.Scanning;

public class FilePathScanHandler : IScanHandler
{
    private readonly IOutputWriter _outputWriter;
    private readonly Lazy<ILineMatcher> _lazyLineMatcher;

    public FilePathScanHandler(ILineMatcherFactory lineMatcherFactory, IOutputWriter outputWriter)
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
    }

    /// <inheritdoc />
    public void ScanSource(ISourceInput sourceInput)
    {
        var lineMatcher = _lazyLineMatcher.Value;
        
        while (sourceInput.TryReadNext(out var content))
        {
            if (!lineMatcher.IsMatch(content)) continue;
            
            _outputWriter.WriteFileInfo(sourceInput.SourceId);
            _outputWriter.WriteLine();
            return;
        }
    }
}