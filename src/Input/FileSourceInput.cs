using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Input;

/// <summary>
/// Represents a file source input.
/// </summary>
public class FileSourceInput : ISourceInput
{
    private readonly ILogger _logger;
    private readonly bool _trackBytes;
    private readonly bool _throwErrors;
    private long _bytesRead;
    private Lazy<StreamReader?> _lazyStreamReader;

    public FileSourceInput(IOptions<ScanOptions> options, ILogger<FileSourceInput> logger, string path, bool trackBytes)
    {
        _logger = logger;
        _trackBytes = trackBytes;
        _throwErrors = !options.Value.NoMessages;
        _lazyStreamReader = new Lazy<StreamReader?>(TryCreateReader);
        SourceId = path;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _lazyStreamReader.Value?.Dispose();
    }

    /// <inheritdoc />
    public SourceInputContext Context => SourceInputContext.File;

    /// <inheritdoc />
    public string SourceId { get; }

    /// <inheritdoc />
    public long ByteOffset { get; private set; }

    /// <inheritdoc />
    public int LineNumber { get; private set; }

    /// <inheritdoc />
    public bool TryReadNext([NotNullWhen(true)] out string? str)
    {
        str = null;
        var reader = _lazyStreamReader.Value;
        if (reader == null) return false;

        ByteOffset = _bytesRead;
        LineNumber++;
        str = reader.ReadLine();
        _bytesRead += ComputeBytesRead(reader, str);
        return str != null;
    }

    /// <inheritdoc />
    public override string ToString() => SourceId;

    private StreamReader? TryCreateReader()
    {
        try
        {
            return new StreamReader(new FileStream(SourceId, FileMode.Open));
        }
        catch (Exception exception)
        {
            if (_throwErrors)
                throw;

            _logger.LogWarning("{Message}", exception.Message);
            return null;
        }
    }

    private long ComputeBytesRead(StreamReader reader, string? str)
    {
        return _trackBytes && str != null
            ? reader.CurrentEncoding.GetByteCount(str)
            : 0;
    }
}