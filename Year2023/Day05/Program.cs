using Day05;
using Util.Aoc;

var challenge = new Challenge(2023, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Nearest location", PartOne);
var two = new Part<int>("N/A", PartTwo);

Console.WriteLine(one.Test(-1, example));
//Console.WriteLine(two.Test(-1, example));

//Console.WriteLine(one.Run(actual));
//Console.WriteLine(two.Run(actual));

int PartOne(string input)
{
    var emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    var blocks = input.Split(emptyLine);
    var seeds = ParseSeeds(blocks.First());
    var almanac = Almanac.Parse(blocks.Skip(1));

    Console.WriteLine($"seeds: {string.Join(' ', seeds)}");
    Console.WriteLine();
    Console.WriteLine(almanac);

    return 0;
}

IEnumerable<uint> ParseSeeds(string line)
{
    var seeds = line.Split(": ")[1];
    return seeds.Split(' ').Select(uint.Parse);
}

int PartTwo(string input)
{
    return 0;
}