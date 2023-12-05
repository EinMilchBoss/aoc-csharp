using Day03;
using Util.Aoc;

var challenge = new Challenge(2023, 3);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of adjacent part numbers", PartOne);
var two = new Part<int>("Sum of all part numbers of gears", PartTwo);

Console.WriteLine(one.Test(4361, example));
Console.WriteLine(two.Test(467835, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

int PartOne(string input)
{
    var grid = new Grid(input);
    var partNumbers = grid.FindPartNumbers();
    var symbols = grid.FindSymbols();
    return partNumbers
        .Where((part) => part.HasAdjacentSymbol(symbols))
        .Select(grid.ParseNumber)
        .Sum();
}

int PartTwo(string input)
{
    var grid = new Grid(input);
    var partNumbers = grid.FindPartNumbers();
    return grid.GetGearNumbers(partNumbers)
        .Select(grid.Ratio)
        .Sum();
}