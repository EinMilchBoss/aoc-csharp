namespace Day03;

public readonly record struct Position(int X, int Y)
{
    public bool IsInRange(PartNumber number)
    {
        var area = GetAdjacentArea();
        return number.GetCoveredPositions()
            .Any((position) => area.Contains(position));
    }

    private IEnumerable<Position> GetAdjacentArea()
    {
        var (ownX, ownY) = this;
        return Enumerable.Range(ownX - 1, 3)
            .SelectMany((x) => Enumerable.Range(ownY - 1, 3)
                .Select((y) => new Position(x, y))
            );
    }
}