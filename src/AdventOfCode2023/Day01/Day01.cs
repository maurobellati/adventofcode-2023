namespace adventofcode2023.Day01;

public static class Day01
{
    public static int Part1(string file) => File.ReadAllLines(file).Select(ExtractFirstAndLastDigits).Sum();

    public static int Part2(string file) => File.ReadAllLines(file).Select(ExtractFirstAndLastWordDigit).Sum();

    private static int ExtractFirstAndLastDigits(string line)
    {
        var firstIndex = line.IndexOfAny("0123456789".ToArray());
        var lastIndex = line.LastIndexOfAny("0123456789".ToArray());
        var first = line[firstIndex];
        var last = line[lastIndex];

        return int.Parse($"{first}{last}");
    }

    private static int ExtractFirstAndLastWordDigit(string line)
    {
        Dictionary<string, int> replacements = new()
        {
            { "0", 0 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        var first = replacements
            .Select(kv => (kv.Value, Index: line.IndexOf(kv.Key, StringComparison.CurrentCultureIgnoreCase)))
            .Where(it => it.Index >= 0)
            .MinBy(it => it.Index)
            .Value;
        var last = replacements
            .Select(kv => (kv.Value, Index: line.LastIndexOf(kv.Key, StringComparison.CurrentCultureIgnoreCase)))
            .Where(it => it.Index >= 0)
            .MaxBy(it => it.Index)
            .Value;

        return first * 10 + last;
    }
}
