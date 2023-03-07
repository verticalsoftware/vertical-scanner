using Vertical.Scanner.Input;

namespace Vertical.Scanner.Scanning;

/// <summary>
/// Abstracts scanning source input content
/// </summary>
public interface IScanHandler
{
    /// <summary>
    /// Signals that scanning is starting.
    /// </summary>
    void SourceScanStarting();

    /// <summary>
    /// Signals that scanning is completed.
    /// </summary>
    void SourceScanCompleted();
    
    /// <summary>
    /// Scan the source for matches.
    /// </summary>
    /// <param name="sourceInput">New source input</param>
    void ScanSource(ISourceInput sourceInput);
}