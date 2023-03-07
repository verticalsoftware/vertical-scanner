using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Output;

public class SurroundRegionOutputController : OutputController, IOutputController
{
    private readonly Queue<WriteDeferredSection> _sectionBuffer = new();
    private readonly Queue<WriteDeferredLine> _lineBuffer = new();
    private readonly SurroundRegion _surround;
    private int _matchCount;
    private int _surroundTailLinesRemaining;

    /// <inheritdoc />
    public SurroundRegionOutputController(
        ScanOptions scanOptions,
        ISourceInput sourceInput,
        IOutputWriter writer,
        bool writePaths) : base(scanOptions, sourceInput, writer, writePaths)
    {
        _surround = scanOptions.GetSurroundRegion();
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
            
            case MatchSectionType.NonMatch:
                _sectionBuffer.Enqueue(new WriteDeferredSection(MatchSectionType.NonMatch, section.Value.ToString()));
                break;
        }
    }

    /// <inheritdoc />
    public void LineScanCompleted()
    {
        try
        {
            if (_matchCount > 0)
            {
                TryWritePathInfo();
                while (_lineBuffer.TryDequeue(out var lineEntry))
                {
                    WriteLinePreamble(lineEntry.ByteOffset, lineEntry.LineNumber);
                    WriteDeferredLineSections(lineEntry.Sections);
                    Writer.TryWriteLine();
                }
                WriteLinePreamble();
                WriteDeferredLineSections(_sectionBuffer.ToArray());
                Writer.TryWriteLine();
                _surroundTailLinesRemaining = _surround.CountAfter;
                return;
            }

            if (--_surroundTailLinesRemaining >= 0)
            {
                WriteLinePreamble();
                WriteDeferredLineSections(_sectionBuffer.ToArray());
                Writer.TryWriteLine();
                return;
            }

            if (_sectionBuffer.Count > 0 && _surround.CountBefore > 0)
            {
                var lineEntry = new WriteDeferredLine(
                    _sectionBuffer.ToArray(), 
                    SourceInput.ByteOffset,
                    SourceInput.LineNumber);
                
                _lineBuffer.Enqueue(lineEntry);
                
                while (_lineBuffer.Count > _surround.CountBefore)
                {
                    _lineBuffer.Dequeue();
                }
            }
        }
        finally
        {
            _sectionBuffer.Clear();
            _matchCount = 0;
        }
    }
}