using Util.Aoc;

var challenge = new Challenge(2024, 2);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Amount of safe reports", PartOne);
var two = new Part<int>("Amount of tolerated reports", PartTwo);

Console.WriteLine(one.Test(2, example));
Console.WriteLine(two.Test(4, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

int PartOne(string input)
{
    var reports = ParseReports(input);
    return reports.Count(IsReportValid);
}

int PartTwo(string input)
{
    var reports = ParseReports(input);
    return reports.Count(report =>
    {
        if (IsReportValid(report))
            return true;
                
        return Enumerable.Range(0, report.Length)
            .Any(i =>
            {
                var adjustedReport = RemoveIndex(report, i);
                return IsReportValid(adjustedReport);
            });
    });
}

int[][] ParseReports(string input) 
{
    return input
        .Split(Environment.NewLine)
        .Select(line => line
            .Split(' ')
            .Select(int.Parse)
            .ToArray()
        )
        .ToArray();
}

bool IsReportValid(int[] report)
{
    var levelDifferences = GetDifferences(report);
    if (!AreDeltasValid(levelDifferences))
        return false;
    
    var positiveLevels = levelDifferences.Count(int.IsPositive);
    var negativeLevels = levelDifferences.Count(int.IsNegative);
    return positiveLevels == 0 || negativeLevels == 0;
}

int[] GetDifferences(int[] values)
{
    return Enumerable.Range(0, values.Length - 1)
        .Select(i => values[i] - values[i + 1])
        .ToArray();
}

bool AreDeltasValid(int[] differences)
{
    return differences.All(difference =>
    {
        var delta = Math.Abs(difference);
        return delta is >= 1 and <= 3;
    });
}

int[] RemoveIndex(int[] oldValues, int removingIndex)
{
    var newValues = new int[oldValues.Length - 1];
    
    for (var i = 0; i < removingIndex; i++)
        newValues[i] = oldValues[i];
    for (var i = removingIndex + 1; i < oldValues.Length; i++)
        newValues[i - 1] = oldValues[i];
    
    return newValues;
}