using Day02;
using Util.Aoc;

var challenge = new Challenge(2023, 2);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of possible games", PartOne);
var two = new Part<int>("Power of minimal game sets", PartTwo);

Console.WriteLine(one.Test(8, example));
Console.WriteLine(two.Test(2286, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

int PartOne(string input) => input.Split(Environment.NewLine)
    .Select(Game.FromString)
    .Where((game) => game.IsPossible())
    .Select((game) => game.Id)
    .Sum();

int PartTwo(string input) => input.Split(Environment.NewLine)
    .Select((line) => Game.FromString(line).MinimalSet().Power())
    .Sum();