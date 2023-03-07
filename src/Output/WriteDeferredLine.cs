namespace Vertical.Scanner.Output;

public record WriteDeferredLine(WriteDeferredSection[] Sections, long ByteOffset, int LineNumber);