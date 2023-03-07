using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;
using Vertical.Scanner.Output;

namespace Vertical.Scanner.Scanning;

public class LineMatchingScanHandler : IScanHandler
{
    private readonly IOutputControllerFactory _outputControllerFactory;
    private readonly Lazy<ILineMatcher> _lazyLineMatcher;
    private readonly ScanOptions _scanOptions;

    public LineMatchingScanHandler(
        ILineMatcherFactory lineMatcherFactory,
        IOutputControllerFactory outputControllerFactory,
        IOptions<ScanOptions> options)
    {
        _outputControllerFactory = outputControllerFactory;
        _lazyLineMatcher = new Lazy<ILineMatcher>(lineMatcherFactory.CreateLineMatcher);
        _scanOptions = options.Value;
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
        using var outputController = _outputControllerFactory.CreateController(sourceInput);
        
        ScanInternal(sourceInput, outputController);
    }

    private void ScanInternal(ISourceInput sourceInput, IOutputController outputController)
    {
        var matcher = _lazyLineMatcher.Value;
        var matchedLineCount = 0;

        while (sourceInput.TryReadNext(out var content))
        {
            var matchedSectionCount = 0;

            matcher.ScanLine(content, (in MatchSection section) =>
            {
                if (section.Type == MatchSectionType.Match)
                    ++matchedSectionCount;
                
                outputController.HandleMatchedSection(section);                                                        
            });
            
            outputController.LineScanCompleted();

            if (_scanOptions.MaxCount > -1 
                && matchedSectionCount > 0 
                && ++matchedLineCount > _scanOptions.MaxCount)
                return;
        }
    }
}