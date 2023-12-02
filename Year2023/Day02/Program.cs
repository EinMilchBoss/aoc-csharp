using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Util.Aoc;

var challenge = new Challenge(2023, 2);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Calibration sum", PartOne);
var two = new Part<int>("Advanced calibration sum", PartTwo);

Console.WriteLine(one.Test(8, example));
//Console.WriteLine(two.Test(-1, example));

Console.WriteLine(one.Run(actual));
//Console.WriteLine(two.Run(actual));

int PartOne(string input)
{
    const int MAX_R = 12;
    const int MAX_G = 13;
    const int MAX_B = 14;
    Console.WriteLine(input);
    // loop through every line
    // if any number for a color of any set is not possible
    // - add id to sum
    var sum = 0;
    foreach (var line in input.Split(Environment.NewLine))
    {
        var gameToSet = line.Split(": ");
        var set = gameToSet[1];
        var subsets = set.Split("; ")
            .Select((s) =>
            {
                var values = s.Split(", ")
                .Select((v) => v.Split(" "));
                int r = 0, g = 0, b = 0;
                foreach (var v in values)
                {

                    switch (v)
                    {
                        case [var x, "red"]:
                            r = int.Parse(x);
                            break;
                        case [var x, "green"]:
                            g = int.Parse(x);
                            break;
                        case [var x, "blue"]:
                            b = int.Parse(x);
                            break;
                        default:
                            break;
                    }
                }

                return new Subset(r, g, b);
            });
        //subsets.ToList().ForEach(s => Console.WriteLine(s));

        if (!subsets.Any((ss) =>
        {
            return ss.r > MAX_R || ss.g > MAX_G || ss.b > MAX_B;
        }))
        {

            var game = gameToSet[0];
            var id = int.Parse(game.Split(" ")[1]);
            sum += id;

            Console.WriteLine(id);
        }
    }

    return sum;
}

int PartTwo(string input)
{
    return 0;
}


readonly record struct Subset(int r, int g, int b);