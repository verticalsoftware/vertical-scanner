using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Scanning;

namespace Vertical.Scanner.Execution;

public class ScannerImplementation : IEntryPoint
{
    private readonly ILogger _logger;
    private readonly IEnumerable<ISourceInput> _sourceInputs;
    private readonly IScanHandler _scanHandler;
    private readonly ScanOptions _options;

    public ScannerImplementation(
        ILogger<ScannerImplementation> logger,
        IEnumerable<ISourceInput> sourceInputs,
        IScanHandler scanHandler,
        IOptions<ScanOptions> options)
    {
        _logger = logger;
        _sourceInputs = sourceInputs;
        _scanHandler = scanHandler;
        _options = options.Value;
    }

    private void LogOptions()
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Parsed arguments:\n{Options}",
                JsonSerializer.Serialize(_options, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
            
            _logger.LogDebug("Using scanner type {ScannerType}", _scanHandler.GetType().Name);
        }
    }

    /// <inheritdoc />
    public bool Handles() => true;

    /// <inheritdoc />
    public void Execute()
    {
        LogOptions();
        
        _scanHandler.SourceScanStarting();

        foreach (var source in _sourceInputs)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Scanning source {Id}", source.SourceId);
            }
            _scanHandler.ScanSource(source);
            source.Dispose();
        }
        
        _scanHandler.SourceScanCompleted();
    }
}