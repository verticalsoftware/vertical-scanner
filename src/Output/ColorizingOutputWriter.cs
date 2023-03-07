using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.Scanner.Templates;

namespace Vertical.Scanner.Output;

public class ColorizingOutputWriter : IOutputWriter
{
    private readonly Queue<char> _charQueue = new();
    private readonly Dictionary<string, string> _cachedMatchedValueTemplates = new();
    private readonly Queue<string> _availableMatchedValueTemplates;
    private readonly TemplateOptions _templates;

    public ColorizingOutputWriter(IOptions<TemplateOptions> templateOptions)
    {
        _templates = templateOptions.Value;
        _availableMatchedValueTemplates = new Queue<string>(templateOptions.Value.Matched);
    }

    /// <inheritdoc />
    public int CharPos => Console.CursorLeft;

    /// <inheritdoc />
    public void WriteFileInfo(string path)
    {
        WriteContent(_templates.FilePaths, path);
    }

    /// <inheritdoc />
    public void WriteLine()
    {
        WriteValue("{0}", Environment.NewLine);
    }

    /// <inheritdoc />
    public void WriteMatch(string value)
    {
        if (!_cachedMatchedValueTemplates.TryGetValue(value, out var template))
        {
            template = _availableMatchedValueTemplates.Count > 0
                ? _availableMatchedValueTemplates.Dequeue()
                : _templates.Matched.LastOrDefault() ?? "[green]{0}[/]";
            _cachedMatchedValueTemplates.Add(value, template);
        }
        WriteContent(template, value);
    }

    /// <inheritdoc />
    public void WriteNonMatch(string value)
    {
        WriteContent(_templates.NonMatched, value);
    }

    /// <inheritdoc />
    public void WriteByteOffset(long offset)
    {
        WriteValue(_templates.ByteOffsets, offset);
        EnqueueSpace();
    }

    /// <inheritdoc />
    public void WriteLineNumber(int lineNumber)
    {
        WriteValue(_templates.LineNumbers, lineNumber);
        EnqueueSpace();
    }

    private void WriteContent(string format, string content)
    {
        WriteValue(format, content.EscapeMarkup());
    }

    private void WriteValue(string format, object value)
    {
        FlushCharQueue();
        AnsiConsole.Markup(format, value);
    }

    private void FlushCharQueue()
    {
        while (_charQueue.TryDequeue(out var c))
        {
            AnsiConsole.Write(c);
        }
    }

    private void EnqueueSpace() => _charQueue.Enqueue(' ');
}