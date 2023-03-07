using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Output;

/// <summary>
/// Controls output from scan handler to output writer.
/// </summary>
public interface IOutputController : IDisposable
{
    void HandleMatchedSection(in MatchSection section);

    void LineScanCompleted();
}