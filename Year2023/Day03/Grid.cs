using System.Security.Cryptography.X509Certificates;

namespace Day03;

public class Grid
{
    private string[] Lines { get; init; }

    public Grid(string values)
    {
        Lines = values.Split(Environment.NewLine);
    }

    public int ParseNumber(PartNumber number)
    {
        var (x, y) = number.Location;
        return int.Parse(Lines[y].Substring(x, number.Length));
    }

    public IEnumerable<PartNumber> FindPartNumbers() => Enumerable.Range(0, Lines.Length)
        .SelectMany(FindPartNumbersInLine);

    private IEnumerable<PartNumber> FindPartNumbersInLine(int lineIndex)
    {
        var numbers = new List<PartNumber>();
        for (var charIndex = 0; charIndex < Lines[lineIndex].Length; charIndex += 1)
        {
            if (!char.IsDigit(Lines[lineIndex][charIndex])) continue;

            var length = GetPartNumberLength(Lines[lineIndex], charIndex);
            numbers.Add(new PartNumber
            (
                new Position(charIndex, lineIndex),
                length
            ));

            charIndex += length;
        }
        return numbers;
    }

    private int GetPartNumberLength(string line, int start)
    {
        var length = 1;
        try
        {
            while (char.IsDigit(line[start + length]))
            {
                length += 1;
            }
        }
        catch (IndexOutOfRangeException)
        { }
        return length;
    }

    public IEnumerable<Position> FindSymbols()
    {
        var symbols = new List<Position>();
        for (var y = 0; y < Lines.Length; y += 1)
        {
            for (var x = 0; x < Lines[y].Length; x += 1)
            {
                if (!IsSymbol(Lines[y][x])) continue;

                symbols.Add(new Position(x, y));
            }
        }
        return symbols;
    }

    private bool IsSymbol(char c) => !char.IsDigit(c) && c != '.';
}