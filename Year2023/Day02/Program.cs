using Day02;
using Util.Aoc;

var challenge = new Challenge(2023, 2);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of possible games", PartOne);
var two = new Part<int>("N/A", PartTwo);

Console.WriteLine(one.Test(8, example));
//Console.WriteLine(two.Test(-1, example));

Console.WriteLine(one.Run(actual));
//Console.WriteLine(two.Run(actual));

int PartOne(string input) => input.Split(Environment.NewLine)
    .Select(Game.FromString)
    .Where((game) => game.IsPossible())
    .Select((game) => game.Id)
    .Sum();

int PartTwo(string input)
{
    return 0;
}