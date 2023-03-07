namespace Vertical.Scanner.Templates;

public interface ITemplateManager
{
    TemplateOptions GetOptions();

    void SetTemplateValue(string key, string value);
}