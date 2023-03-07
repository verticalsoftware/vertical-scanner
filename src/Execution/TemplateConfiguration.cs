using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.Scanner.Configuration;
using Vertical.Scanner.Templates;

namespace Vertical.Scanner.Execution;

public class TemplateConfiguration : IEntryPoint
{
    private readonly ITemplateManager _templateManager;
    private readonly ScanOptions _options;

    public TemplateConfiguration(IOptions<ScanOptions> options, ITemplateManager templateManager)
    {
        _templateManager = templateManager;
        _options = options.Value;
    }
    
    /// <inheritdoc />
    public bool Handles()
    {
        return _options.GetTemplates || _options.SetTemplate != null;
    }

    /// <inheritdoc />
    public void Execute()
    {
        switch (_options)
        {
            case { GetTemplates: true }:
                GetTemplates();
                break;
            
            case { SetTemplate: {} }:
                SetTemplate();
                break;
        }
    }

    private void SetTemplate()
    {
        var match = Regex.Match(_options.SetTemplate!, @"(\w+)=(.+)");
        if (!match.Success)
        {
            AnsiConsole.MarkupLine("Invalid --set-template parameter [red]'{0}'[/red]",
                _options.SetTemplate!.EscapeMarkup());
            return;
        }
        
        _templateManager.SetTemplateValue(match.Groups[1].Value, match.Groups[2].Value);
    }

    private void GetTemplates()
    {
        var options = _templateManager.GetOptions();
        AnsiConsole.MarkupLine("{0,-20}{1,-30}{2}", "ID", "Template", "Sample"); 
        AnsiConsole.WriteLine("---------------------------------------------------");
        WriteTemplate(nameof(TemplateOptions.ByteOffsets), options.ByteOffsets);
        WriteTemplate(nameof(TemplateOptions.FilePaths), options.FilePaths);
        WriteTemplates(nameof(TemplateOptions.Matched), options.Matched);
        WriteTemplate(nameof(TemplateOptions.NonMatched), options.NonMatched);
        WriteTemplate(nameof(TemplateOptions.LineNumbers), options.LineNumbers);
    }

    private void WriteTemplate(string key, string value)
    {
        AnsiConsole.Markup("{0,-20}[olive]{1,-30}[/]", key.EscapeMarkup(), value.EscapeMarkup());
        AnsiConsole.MarkupLine(value, "(sample-output)");
    }

    private void WriteTemplates(string key, string[] values)
    {
        for (var c = 0; c < values.Length; c++)
        {
            WriteTemplate($"{key}{c}", values[c]);
        }
    }
}