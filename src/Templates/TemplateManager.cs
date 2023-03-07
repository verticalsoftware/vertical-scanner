using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Templates;

public class TemplateManager : ITemplateManager
{
    private static readonly string DefaultFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".vertical",
        "scanner",
        "templates.json");

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };
    
    private readonly IOptions<ScanOptions> _options;
    private readonly Lazy<TemplateOptions> _lazyTemplateOptions;

    public TemplateManager(IOptions<ScanOptions> options)
    {
        _options = options;
        _lazyTemplateOptions = new Lazy<TemplateOptions>(LoadSettings);
    }


    private TemplateOptions LoadSettings()
    {
        var path = _options.Value.TemplateFile ?? DefaultFilePath;

        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<TemplateOptions>(json, SerializerOptions)
                   ?? throw new JsonException("Invalid template file");
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine("[red]{0}[/]", exception.Message.EscapeMarkup());
            AnsiConsole.WriteLine("Scanner will use out-of-box templates:");
            return new TemplateOptions();
        }
    }

    /// <inheritdoc />
    public TemplateOptions GetOptions() => _lazyTemplateOptions.Value;

    /// <inheritdoc />
    public void SetTemplateValue(string key, string value)
    {
        var options = GetOptions();

        try
        {
            AnsiConsole.Markup("Template will be rendered like so: ");
            AnsiConsole.MarkupLine(value, "<-- Test value -->");
        }
        catch
        {
            AnsiConsole.MarkupLine("Value [red]'{0}[/]' is not valid SpectreConsole markup",
                value.EscapeMarkup());
            return;
        }

        var match = Regex.Match(key, nameof(TemplateOptions.Matched) + @"(\d+)");
        
        if (match.Success)
        {
            var index = int.Parse(match.Groups[1].Value);
            if (index < options.Matched.Length)
            {
                options.Matched[index] = value;
            }
            else
            {
                AnsiConsole.WriteLine("Could not set {0}[[{1}]] value because it is not in the valid index range",
                    nameof(TemplateOptions.Matched),
                    index);
                return;
            }
        }
        else
        {
            switch (key)
            {
                case nameof(TemplateOptions.ByteOffsets):
                    options.ByteOffsets = value;
                    break;
                
                case nameof(TemplateOptions.FilePaths):
                    options.FilePaths = value;
                    break;
                
                case nameof(TemplateOptions.LineNumbers):
                    options.LineNumbers = value;
                    break;
                
                case nameof(TemplateOptions.NonMatched):
                    options.NonMatched = value;
                    break;
            }
        }

        var path = _options.Value.TemplateOut ?? DefaultFilePath;
        var json = JsonSerializer.Serialize(options, SerializerOptions);
        var directory = Path.GetDirectoryName(Path.GetFullPath(path))!;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        File.WriteAllText(path, json);
        AnsiConsole.MarkupLine("Wrote template file at [olive]{0}[/]", path.EscapeMarkup());
    }
}