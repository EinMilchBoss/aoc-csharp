using Day04;
using Util.Aoc;

var challenge = new Challenge(2023, 4);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of card points", PartOne);
var two = new Part<int>("Amount of total cards won", PartTwo);

Console.WriteLine(one.Test(13, example));
Console.WriteLine(two.Test(30, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

int PartOne(string input) => input
    .Split(Environment.NewLine)
    .Select((line) => Card.Parse(line).GetPoints())
    .Sum();

int PartTwo(string input) => input
    .Split(Environment.NewLine)
    .Select(Card.Parse)
    .TotalAmountOfCards();