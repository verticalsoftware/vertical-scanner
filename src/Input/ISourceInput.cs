using System.Diagnostics.CodeAnalysis;

namespace Vertical.Scanner.Input;

/// <summary>
/// Abstracts source input.
/// </summary>
public interface ISourceInput : IDisposable
{
    /// <summary>
    /// Gets the source input context.
    /// </summary>
    SourceInputContext Context { get; }
    
    /// <summary>
    /// Gets the id of the input source.
    /// </summary>
    string SourceId { get; }
    
    /// <summary>
    /// Gets the current byte offset.
    /// </summary>
    long ByteOffset { get; }
    
    /// <summary>
    /// Gets the current line number.
    /// </summary>
    int LineNumber { get; }

    /// <summary>
    /// Tries to read a line from the source.
    /// </summary>
    /// <param name="str">If <c>true</c> and not null, the line content.</param>
    /// <returns><c>true</c> if a line was read and was not null.</returns>
    bool TryReadNext([NotNullWhen(true)] out string? str);
}