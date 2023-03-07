using System.Diagnostics.CodeAnalysis;

namespace Vertical.Scanner.Input;

/// <summary>
/// Input coming from 
/// </summary>
public class StandardSourceInput : ISourceInput
{
    /// <inheritdoc />
    public SourceInputContext Context => SourceInputContext.StandardInput;

    /// <inheritdoc />
    public string SourceId => "-";

    /// <inheritdoc />
    public long ByteOffset { get; private set; }

    /// <inheritdoc />
    public int LineNumber { get; private set; }

    /// <inheritdoc />
    public bool TryReadNext([NotNullWhen(true)] out string? str)
    {
        str = Console.ReadLine();
        ByteOffset += str?.Length * 2 ?? 0;
        LineNumber++;
        return str != null;
    }
    
    /// <summary>
    /// Not implemented.
    /// </summary>
    public void Dispose() {}

    /// <inheritdoc />
    public override string ToString() => "(StdIn)";
}