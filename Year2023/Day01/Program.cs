using Util.Aoc;

Dictionary<string, int> NUMBER_WORDS = new()
{
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3,
    ["four"] = 4,
    ["five"] = 5,
    ["six"] = 6,
    ["seven"] = 7,
    ["eight"] = 8,
    ["nine"] = 9,
};

var challenge = new Challenge(2023, 1);
var example1 = challenge.ReadInput("example1.txt");
var example2 = challenge.ReadInput("example2.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Calibration sum", PartOne);
var two = new Part<int>("Advanced calibration sum", PartTwo);

Console.WriteLine(one.Test(142, example1));
Console.WriteLine(two.Test(281, example2));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

int PartOne(string input)
{
    return input.Split(Environment.NewLine)
        .Select((line) =>
        {
            var (_, first) = FindFirstNumber(line)!.Value;
            var (_, last) = FindLastNumber(line)!.Value;
            return first * 10 + last;
        })
        .Sum();
}

int PartTwo(string input)
{
    /*
    For part two they provided an example input that, for some reason, contains line(s) that have no number in them.
    Therefore we have to return and check for null values every time.
    We could theoretically switch the check for number words with numbers, but that would imply runtime cost.
    Not that it would matter, but you know...
    */

    return input.Split(Environment.NewLine)
        .Select((line) =>
        {
            var first = 0;
            var last = 0;

            var firstIndex = line.Length - 1;
            var lastIndex = 0;

            if (FindFirstNumber(line) is Result firstResult)
            {
                first = firstResult.Number;
                firstIndex = firstResult.Index;
            }
            if (FindFirstNumberWord(line, firstIndex) is int newFirst)
            {
                first = newFirst;
            }

            if (FindLastNumber(line) is Result lastResult)
            {
                last = lastResult.Number;
                lastIndex = lastResult.Index;
            }
            if (FindLastNumberWord(line, lastIndex) is int newLast)
            {
                last = newLast;
            }

            return first * 10 + last;
        })
        .Sum();
}

Result? FindFirstNumber(string line)
{
    for (var i = 0; i < line.Length; i += 1)
    {
        if (char.IsDigit(line[i]))
        {
            return new Result(i, line[i] - '0');
        }
    }
    return null;
}

Result? FindLastNumber(string line)
{
    for (var i = line.Length - 1; i >= 0; i -= 1)
    {
        if (char.IsDigit(line[i]))
        {
            return new Result(i, line[i] - '0');
        }
    }
    return null;
}

int? FindFirstNumberWord(string line, int until)
{
    for (var i = 0; i < until; i += 1)
    {
        foreach (var word in NUMBER_WORDS.Keys)
        {
            if (line[i..].StartsWith(word))
            {
                return NUMBER_WORDS[word];
            }
        }
    }
    return null;
}

int? FindLastNumberWord(string line, int until)
{
    for (var i = line.Length - 1; i > until; i -= 1)
    {
        foreach (var word in NUMBER_WORDS.Keys)
        {
            if (line[i..].StartsWith(word))
            {
                return NUMBER_WORDS[word];
            }
        }
    }
    return null;
}

readonly record struct Result(int Index, int Number);