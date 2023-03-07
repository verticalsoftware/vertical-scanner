using Microsoft.Extensions.Options;

namespace Vertical.Scanner.Templates;

public class TemplateOptionsProvider : IOptions<TemplateOptions>
{
    private readonly ITemplateManager _templateManager;

    /// <summary>
    /// Creates a new instance of this type
    /// </summary>
    public TemplateOptionsProvider(ITemplateManager templateManager)
    {
        _templateManager = templateManager;
    }

    /// <inheritdoc />
    public TemplateOptions Value => _templateManager.GetOptions();
}