namespace adventofcode2023.Day01;

public static class Day01
{
    public static void Solve(string[] args)
    {
        var input = File.ReadAllLines($"Day01/{args.FirstOrDefault() ?? "input.txt"}");

        Console.WriteLine($"Part 1: {Part1(input)}");
        Console.WriteLine($"Part 2: {Part2(input)}");
    }

    private static int Part1(IEnumerable<string> input) => input.Select(ExtractFirstAndLastDigits).Sum();

    private static int Part2(IEnumerable<string> input) => input.Select(ExtractFirstAndLastWordDigit).Sum();

    private static int ExtractFirstAndLastDigits(string line)
    {
        var firstIndex = line.IndexOfAny("0123456789".ToArray());
        var lastIndex = line.LastIndexOfAny("0123456789".ToArray());
        var first = (int)char.GetNumericValue(line[firstIndex]);
        var last = (int)char.GetNumericValue(line[lastIndex]);

        var result = first * 10 + last;
        Console.WriteLine($"{line} -> {first} + {last} = {result}");
        return result;
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

        var result = first * 10 + last;
        Console.WriteLine($"{line} -> {first} + {last} = {result}");
        return result;
    }
}
