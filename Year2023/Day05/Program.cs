using Util.Aoc;

var challenge = new Challenge(2023, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Nearest location", PartOne);
var two = new Part<int>("N/A", PartTwo);

Console.WriteLine(one.Test(-1, example));
//Console.WriteLine(two.Test(-1, example));

Console.WriteLine(one.Run(actual));
//Console.WriteLine(two.Run(actual));

int PartOne(string input)
{
    Console.WriteLine(input);

    return 0;
}

int PartTwo(string input)
{
    return 0;
}