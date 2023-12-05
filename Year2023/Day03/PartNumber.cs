namespace Day03;

public readonly record struct PartNumber(Position Location, int Length)
{
    public int MinX => Location.X;
    public int MaxX => Location.X + Length - 1;
    public int Y => Location.Y;

    public bool HasAdjacentSymbol(IEnumerable<Position> symbols) => symbols.Any(IsInRange);

    private bool IsInRange(Position symbol)
    {
        var isWidthInRange = MinX - 1 <= symbol.X && symbol.X <= MaxX + 1;
        var isHeightInRange = Y - 1 <= symbol.Y && symbol.Y <= Y + 1;
        return isWidthInRange && isHeightInRange;
    }

    public IEnumerable<Position> GetCoveredPositions()
    {
        var (ownX, ownY) = Location;
        return Enumerable.Range(ownX, Length)
            .Select((x) => new Position(x, ownY));
    }
}