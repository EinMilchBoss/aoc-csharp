using Util.Aoc;

var challenge = new Challenge(2024, 1);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of location ID deltas", PartOne);
var two = new Part<int>("Sum of similarity score", PartTwo);

Console.WriteLine(one.Test(11, example));
Console.WriteLine(two.Test(31, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

(List<int>, List<int>) ParseValues(string input)
{
    var lines = input.Split(Environment.NewLine);
    
    var leftValues = new List<int>();
    var rightValues = new List<int>();
    
    foreach (var line in lines)
    {
        var values = line.Split("   ");
        
        leftValues.Add(int.Parse(values[0]));
        rightValues.Add(int.Parse(values[1]));
    }
    
    return (leftValues, rightValues);
}

int PartOne(string input)
{
    var (leftValues, rightValues) = ParseValues(input);
    
    leftValues.Sort();
    rightValues.Sort();

    return leftValues
        .Select((leftValue, i) =>
        {
            var rightValue = rightValues[i];
            var delta = Math.Abs(leftValue - rightValue);
            return delta;
        })
        .Sum();
}

int PartTwo(string input)
{
    var (leftValues, rightValues) = ParseValues(input);

    var rightOccurrences = rightValues
        .GroupBy(value => value)
        .Select(grouping => (grouping.Key, grouping.Count()))
        .ToDictionary();

    return leftValues.Sum(leftValue => leftValue * rightOccurrences.GetValueOrDefault(leftValue, 0));
}