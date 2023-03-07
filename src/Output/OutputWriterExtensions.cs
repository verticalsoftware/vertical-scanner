namespace Vertical.Scanner.Output;

public static class OutputWriterExtensions
{
    public static void TryWriteLine(this IOutputWriter writer)
    {
        if (writer.CharPos == 0)
            return;

        writer.WriteLine();
    }
}