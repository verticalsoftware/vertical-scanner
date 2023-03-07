namespace Vertical.Scanner.Output;

/// <summary>
/// Abstracts output
/// </summary>
public interface IOutputWriter
{
    /// <summary>
    /// Gets the current character write position.
    /// </summary>
    int CharPos { get; }
    
    void WriteFileInfo(string path);

    void WriteLine();

    void WriteMatch(string value);

    void WriteNonMatch(string value);

    void WriteByteOffset(long offset);

    void WriteLineNumber(int lineNumber);
}