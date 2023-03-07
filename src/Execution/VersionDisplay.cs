using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Execution;

public class VersionDisplay : IEntryPoint
{
    private readonly IOptions<ScanOptions> _options;

    public VersionDisplay(IOptions<ScanOptions> options)
    {
        _options = options;
    }
    
    /// <inheritdoc />
    public bool Handles()
    {
        return _options.Value.PrintVersion;
    }

    /// <inheritdoc />
    public void Execute()
    {
        AnsiConsole.MarkupLine("Vertical.Scanner, [green]v1.0.0[/]");
        AnsiConsole.MarkupLine("[grey]Copyright (C) 2023[/]");
    }
}