using System.Collections.Generic;
using System.Collections.Immutable;
using Day05;
using Util.Aoc;

var challenge = new Challenge(2023, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<uint>("Nearest location of seeds", PartOne);
var two = new Part<uint>("Nearest location of seed ranges", PartTwo);

//Console.WriteLine(one.Test(35, example));
Console.WriteLine(two.Test(46, example));

//Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

uint PartOne(string input)
{
    var emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    var blocks = input.Split(emptyLine);
    var seeds = ParseSeeds(blocks.First());
    var almanac = Almanac.Parse(blocks.Skip(1));

    // Console.WriteLine($"seeds: {string.Join(' ', seeds)}");
    // Console.WriteLine();
    // Console.WriteLine(almanac);

    return seeds.Select(almanac.GetLocation).Min();
}

IEnumerable<uint> ParseSeeds(string line)
{
    var seeds = line.Split(": ")[1];
    return seeds.Split(' ').Select(uint.Parse);
}

IEnumerable<uint> ParseSeedRanges(string line)
{
    var seeds = line.Split(": ")[1];
    var values = seeds
        .Split(' ')
        .Select(uint.Parse)
        .ToImmutableArray();
    var seedValues = new List<uint>();
    for (var i = 0; i < values.Count(); i += 2)
    {
        var temp = new List<uint>(checked((int)values[i + 1]));
        for (var number = values[i]; number < values[i] + values[i + 1]; number += 1)
        {
            temp.Add(number);
        }
        seedValues.AddRange(temp.AsEnumerable());
    }
    return seedValues.AsEnumerable();
}

uint PartTwo(string input)
{
    var emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    var blocks = input.Split(emptyLine);
    var seeds = ParseSeedRanges(blocks.First());
    var almanac = Almanac.Parse(blocks.Skip(1));

    // Console.WriteLine($"seeds: {string.Join(' ', seeds)}");
    // Console.WriteLine();
    // Console.WriteLine(almanac);

    return seeds.Select(almanac.GetLocation).Min();
}