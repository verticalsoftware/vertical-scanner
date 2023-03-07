using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Output;

/// <summary>
/// Writes all output
/// </summary>
public class IndiscriminateOutputController : OutputController, IOutputController
{
    public IndiscriminateOutputController(
        ScanOptions scanOptions,
        ISourceInput sourceInput,
        IOutputWriter writer,
        bool writePaths) : base(scanOptions, sourceInput, writer, writePaths)
    {
    }
    
    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public void HandleMatchedSection(in MatchSection section)
    {
        TryWritePathInfo();
        TryWriteLinePreamble();
        
        switch (section.Type)
        {
            case MatchSectionType.Match:
                Writer.WriteMatch(section.Value.ToString());
                break;
            
            case MatchSectionType.NonMatch:
                Writer.WriteNonMatch(section.Value.ToString());
                break;
        }
    }

    /// <inheritdoc />
    public void LineScanCompleted()
    {
        Writer.TryWriteLine();
        ResetLinePreambleState();
    }
}