namespace Day03;

public readonly record struct PartNumber(Position Location, int Length)
{
    private int MinX => Location.X;
    private int MaxX => Location.X + Length - 1;
    private int Y => Location.Y;

    public bool HasAdjacentSymbol(IEnumerable<Position> symbols) => symbols.Any(IsInRange);

    private bool IsInRange(Position symbol)
    {
        var isWidthInRange = MinX - 1 <= symbol.X && symbol.X <= MaxX + 1;
        var isHeightInRange = Y - 1 <= symbol.Y && symbol.Y <= Y + 1;
        return isWidthInRange && isHeightInRange;
    }
}