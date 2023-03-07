using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Output;

public abstract class OutputController
{
    private bool _writePathInfoEvaluated;
    private bool _writeLinePreambleEvaluated;
    
    protected OutputController(
        ScanOptions scanOptions,
        ISourceInput sourceInput,
        IOutputWriter writer,
        bool writePaths)
    {
        ScanOptions = scanOptions;
        SourceInput = sourceInput;
        Writer = writer;
        WritePaths = writePaths;
    }

    public ScanOptions ScanOptions { get; }

    public ISourceInput SourceInput { get; }

    public IOutputWriter Writer { get; }

    public bool WritePaths { get; }

    protected void TryWritePathInfo()
    {
        if (_writePathInfoEvaluated)
            return;

        _writePathInfoEvaluated = true;

        if (!WritePaths)
            return;
        
        Writer.TryWriteLine();
        Writer.WriteFileInfo($"Results in {SourceInput.SourceId}");
    }

    protected void WriteDeferredLineSections(WriteDeferredSection[] sections)
    {
        for (var c = 0; c < sections.Length; c++)
        {
            WriteDeferredSection(sections[c]);
        }
    }

    protected void WriteDeferredSection(in WriteDeferredSection section)
    {
        switch (section.Type)
        {
            case MatchSectionType.Match:
                Writer.WriteMatch(section.Value);
                break;
            
            case MatchSectionType.NonMatch:
                Writer.WriteNonMatch(section.Value);
                break;
        }
    }

    protected void ResetLinePreambleState()
    {
        _writeLinePreambleEvaluated = false;
    }

    protected void TryWriteLinePreamble()
    {
        if (_writeLinePreambleEvaluated)
            return;

        _writeLinePreambleEvaluated = true;
        WriteLinePreamble();
    }

    protected void WriteLinePreamble(long byteOffset, int lineNumber)
    {
        Writer.TryWriteLine();
        if (ScanOptions.ByteOffsets)
        {
            Writer.WriteByteOffset(byteOffset);
        }

        if (ScanOptions.LineNumbers)
        {
            Writer.WriteLineNumber(lineNumber);
        }
    }

    protected void WriteLinePreamble()
    {
        WriteLinePreamble(SourceInput.ByteOffset, SourceInput.LineNumber);
    }
}