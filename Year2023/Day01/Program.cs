using System.Linq.Expressions;
using Microsoft.VisualBasic;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello World!");

        var example1 = File.ReadAllLines("./res/Year2023/Day01/example1.txt");
        var example2 = File.ReadAllLines("./res/Year2023/Day01/example2.txt");
        var actual = File.ReadAllLines("./res/Year2023/Day01/actual.txt");

        Console.WriteLine($"Part one: {PartOne(example1)}");
        Console.WriteLine($"Part one: {PartOne(actual)}");

        var test = PartTwo(example2);
        Console.WriteLine($"Part two: {test} {test == 281}");
        Console.WriteLine($"Part two: {PartTwo(actual)}");
    }

    public static int PartOne(string[] lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            int first = 0, last = 0;
            for (var i = 0; i < line.Length; i += 1)
            {
                if (char.IsDigit(line[i]))
                {
                    first = line[i] - '0';
                    break;
                }
            }
            for (var i = line.Length - 1; i >= 0; i -= 1)
            {
                if (char.IsDigit(line[i]))
                {
                    last = line[i] - '0';
                    break;
                }
            }
            sum += first * 10 + last;
        }

        return sum;
    }

    public static int PartTwo(string[] lines)
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
            Console.WriteLine(first * 10 + last);

            sum += first * 10 + last;
        }


        return sum;
    }
}