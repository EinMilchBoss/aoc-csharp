using Util.Aoc;

var challenge = new Challenge(2024, 4);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("XMAS anywhere, anyhow", PartOne);
var two = new Part<int>("MAS in form of an X", PartTwo);

Console.WriteLine(one.Test(18, example));
Console.WriteLine(two.Test(9, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

int PartOne(string input)
{
    var grid = ParseGrid(input);

    var sum = 0;
    const char startingChar = 'X';
    const string missingChars = "MAS";
    for (var row = 0; row < grid.Length; row++)
    {
        for (var col = 0; col < grid.Length; col++)
        {
            // I have to do this or my IDE won't stop crying.
            var currentRow = row;
            var currentCol = col;
            
            if (grid[row][col] != startingChar)
                continue;
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow][currentCol + offset]))
                sum += 1;
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow][currentCol - offset]))
                sum += 1;
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow + offset][currentCol]))
                sum += 1;
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow - offset][currentCol]))
                sum += 1;
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow + offset][currentCol + offset]))
                sum += 1;

            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow - offset][currentCol - offset]))
                sum += 1;
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow + offset][currentCol - offset]))
                sum += 1;

            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow - offset][currentCol + offset]))
                sum += 1;
        }
    }

    return sum;
}

int PartTwo(string input)
{
    var grid = ParseGrid(input);

    var pivots = new List<(int x, int y)>();
    char[] missingChars = ['A', 'S'];
    for (var row = 0; row < grid.Length; row++)
    {
        for (var col = 0; col < grid.Length; col++)
        {
            // I have to do this or my IDE won't stop crying.
            var currentRow = row;
            var currentCol = col;
            
            if (grid[row][col] != 'M')
                continue;

            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow + offset][currentCol + offset]))
                pivots.Add((currentRow + 1, currentCol + 1));

            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow - offset][currentCol - offset]))
                pivots.Add((currentRow - 1, currentCol - 1));
            
            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow + offset][currentCol - offset]))
                pivots.Add((currentRow + 1, currentCol - 1));

            if (AreOffsetCharsValid(missingChars, offset => grid[currentRow - offset][currentCol + offset]))
                pivots.Add((currentRow - 1, currentCol + 1));
        }
    }
    
    return pivots
        .GroupBy(pivot => pivot)
        .Count(grouping => grouping.Count() == 2);
}

char[][] ParseGrid(string input)
{
    return input
        .Split(Environment.NewLine)
        .Select(line => line.ToCharArray())
        .ToArray();
}

bool AreOffsetCharsValid(ReadOnlySpan<char> missingChars, Func<int, char> getOffsetChar)
{
    for (var i = 0; i < missingChars.Length; i++)
    {
        try
        {
            var offset = i + 1;
            var offsetChar = getOffsetChar(offset);
            
            if (offsetChar != missingChars[i])
            {
                return false;
            }
        }
        catch (IndexOutOfRangeException)
        {
            // When we reach the end of the line, we cannot hope for more characters.
            return false;
        }
    }

    return true;
}