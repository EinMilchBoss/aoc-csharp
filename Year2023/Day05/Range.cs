namespace Day05;

public readonly record struct Range(uint DestinationStart, uint SourceStart, uint Length)
{
    public static Range Parse(string line)
    {
        var values = line
            .Split(' ')
            .Select(uint.Parse)
            .ToArray();
        return new Range(values[0], values[1], values[2]);
    }

    public override string ToString() => $"{DestinationStart} {SourceStart} {Length}";
}