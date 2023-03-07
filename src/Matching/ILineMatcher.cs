namespace Vertical.Scanner.Matching;

public interface ILineMatcher
{
    /// <summary>
    /// Evaluates the input
    /// </summary>
    /// <param name="input">Input line to evaluate</param>
    /// <param name="callback">Callback that receives each match section</param>
    /// <returns>The number of times the callback was invoked with match data</returns>
    int ScanLine(string input, MatchSectionCallback callback);

    /// <summary>
    /// Evaluates the input.
    /// </summary>
    /// <param name="input">Input line to evaluate</param>
    /// <returns><c>true</c> is any match occurred, otherwise <c>false</c></returns>
    bool IsMatch(string input);
}