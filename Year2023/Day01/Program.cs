using System.Diagnostics;
using Util.Aoc;

public static class Program
{
    public static void Main()
    {
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
    }

    private static int PartOne(string input)
    {
        return input.Split(Environment.NewLine)
            .Select((line) => FindFirstNumber(line) * 10 + FindLastNumber(line))
            .Sum();
    }

    private static int FindFirstNumber(string line)
    {
        for (var i = 0; i < line.Length; i += 1)
        {
            if (char.IsDigit(line[i]))
            {
                return line[i] - '0';
            }
        }
        throw new UnreachableException("Line has to contain at least one number.");
    }

    private static int FindLastNumber(string line)
    {
        for (var i = line.Length - 1; i >= 0; i -= 1)
        {
            if (char.IsDigit(line[i]))
            {
                return line[i] - '0';
            }
        }
        throw new UnreachableException("Line has to contain at least one number.");
    }

    private static int PartTwo(string input)
    {
        var numberWords = new Dictionary<string, int>()
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

        var sum = 0;
        var lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            int first = 0, last = 0;
            int firstIndex = line.Length - 1, lastIndex = 0;

            // durchsuche nach wörtern
            for (var i = 0; i < line.Length; i += 1)
            {
                foreach (var word in numberWords.Keys)
                {
                    //Console.WriteLine($"{i} {word}");
                    if (line.Substring(i).StartsWith(word))
                    {
                        first = numberWords[word];
                        firstIndex = i;
                        goto FoundFirstWord;
                    }
                }
            }
        FoundFirstWord:;

            for (var i = line.Length - 1; i >= 0; i -= 1)
            {
                foreach (var word in numberWords.Keys)
                {
                    if (line.Substring(i).StartsWith(word))
                    {
                        last = numberWords[word];
                        lastIndex = i;
                        goto FoundLastWord;
                    }
                }
            }
        FoundLastWord:;

            // durchsuchen nach zahlen
            for (var i = 0; i <= firstIndex; i += 1)
            {
                if (char.IsDigit(line[i]))
                {
                    first = line[i] - '0';
                    break;
                }
            }

            for (var i = line.Length - 1; i >= lastIndex; i -= 1)
            {
                if (char.IsDigit(line[i]))
                {
                    last = line[i] - '0';
                    break;
                }
            }
            //Console.WriteLine(first * 10 + last);

            sum += first * 10 + last;
        }


        return sum;
    }
}