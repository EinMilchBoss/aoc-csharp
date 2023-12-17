using System.Text;

namespace Day04;

public readonly record struct Card(IEnumerable<int> Winning, IEnumerable<int> Actual)
{
    public static Card Parse(string line)
    {
        var sideOfNumbers = line.Split(": ")[1];
        var numbers = sideOfNumbers.Split(" | ");
        return new Card(ParseNumbers(numbers[0]), ParseNumbers(numbers[1]));
    }

    private static IEnumerable<int> ParseNumbers(string numbers)
    {
        const int NUMBER_LENGTH = 2;

        IEnumerable<int> Iterate(int index)
        {
            if (index + NUMBER_LENGTH > numbers.Length)
                return Enumerable.Empty<int>();

            var number = numbers
                .Substring(index, NUMBER_LENGTH)
                .Replace(' ', '0');
            return Iterate(index + 3).Prepend(int.Parse(number));
        }

        return Iterate(0);
    }

    public int GetPoints()
    {
        var count = GetMatches();
        return (count == 0) ? 0 : (int)Math.Pow(2, count - 1);
    }

    public int GetMatches() => Winning
        .Intersect(Actual)
        .Count();

    public override string ToString() => GetLine(Winning)
        .Append("| ")
        .Append(GetLine(Actual).ToString())
        .ToString();

    private static StringBuilder GetLine(IEnumerable<int> ints) => ints.Aggregate(new StringBuilder(), (a, b) => a.Append($"{b,2} "));
}