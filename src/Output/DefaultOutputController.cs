using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Output;

public class DefaultOutputController : OutputController, IOutputController
{
    private readonly Queue<WriteDeferredSection> _sectionBuffer = new();
    private int _matchCount;
    
    /// <inheritdoc />
    public DefaultOutputController(
        ScanOptions scanOptions,
        ISourceInput sourceInput,
        IOutputWriter writer,
        bool writePaths) 
        : base(scanOptions, sourceInput, writer, writePaths)
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public void HandleMatchedSection(in MatchSection section)
    {
        switch (section.Type)
        {
            case MatchSectionType.Match:
                _matchCount++;
                _sectionBuffer.Enqueue(new WriteDeferredSection(MatchSectionType.Match, section.Value.ToString()));
                break;
            
            case MatchSectionType.NonMatch when !ScanOptions.OnlyMatching:
                _sectionBuffer.Enqueue(new WriteDeferredSection(MatchSectionType.NonMatch, section.Value.ToString()));
                break;
        }
    }

    /// <inheritdoc />
    public void LineScanCompleted()
    {
        try
        {
            if (_sectionBuffer.Count == 0)
                return;

            if (_matchCount == 0)
                return;
            
            TryWritePathInfo();
            WriteLinePreamble();

            while (_sectionBuffer.TryDequeue(out var deferredSection))
            {
                WriteDeferredSection(deferredSection);
            }
            
            Writer.TryWriteLine();
        }
        finally
        {
            _sectionBuffer.Clear();
            _matchCount = 0;
            ResetLinePreambleState();
        }
    }
}