namespace Vertical.Scanner.Templates;

public class TemplateOptions
{
    public string NonMatched { get; set; } = "[blue]{0}[/]";

    public string[] Matched { get; set; } =
    {
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]",
        "[green]{0}[/]"
    };

    public string ByteOffsets { get; set; } = "[purple]{0}[/]";

    public string LineNumbers { get; set; } = "[purple]{0}[/]";

    public string FilePaths { get; set; } = "[olive]{0}[/]";
}