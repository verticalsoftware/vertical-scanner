namespace Vertical.Scanner.Matching;

/// <summary>
/// Creates line matcher for configuration options.
/// </summary>
public interface ILineMatcherFactory
{
    /// <summary>
    /// Creates the line matcher collection.
    /// </summary>
    /// <returns>Collection of <see cref="ILineMatcher"/></returns>
    ILineMatcher CreateLineMatcher();
}