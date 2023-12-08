namespace adventofcode2023.Day03;

using System.Text.RegularExpressions;

public static class Day03
{
    public static long Part1(string file)
    {
        var lines = File.ReadAllLines(file);
        var numbers = lines.SelectMany(ExtractNumbers).ToHashSet();
        var symbols = lines.SelectMany(ExtractSymbols).ToHashSet();
        return numbers.Where(number => symbols.Any(number.IsNearTo))
            .Select(number => number.Value)
            .Sum();
    }

    public static int Part2(string file)
    {
        var lines = File.ReadAllLines(file);
        var allNumbers = lines.SelectMany(ExtractNumbers).ToHashSet();
        var gears = lines.SelectMany(ExtractSymbols).Where(it => it.Value == "*").ToHashSet();

        return gears
            .Select(gear => allNumbers.Where(number => number.IsNearTo(gear)).ToList())
            .Where(numbers => numbers.Count == 2)
            .Select(numbers => numbers[0].Value * numbers[1].Value)
            .Sum();
    }

    private static List<Number> ExtractNumbers(string line, int y) =>
        // line is a string like "467.$..*..114.."
        // extract the numbers from the string, including the start and end position of each number
        Regex.Matches(line, @"\d+")
            .Select(
                match => new Number(
                    int.Parse(match.Value),
                    y,
                    match.Index,
                    match.Index + match.Length - 1
                ))
            .ToList();

    private static List<Symbol> ExtractSymbols(string line, int y) =>
        // line is a string like "467.$..*..114.."
        // a Symbol is any character that is not a number and not a .
        Regex.Matches(line, @"[^.\d]")
            .Select(match => new Symbol(match.Value, new(y, match.Index)))
            .ToList();

    private sealed record Point(int X, int Y);

    private sealed record Number(int Value, int X, int StartY, int EndY)
    {
        public bool IsNearTo(Symbol symbol) => IsNearTo(symbol.Position);

        private bool IsNearTo(Point point) =>
            // a Number is Near to a point if it's inside a box that is 1 point bigger than the Number
            // example: Number(1, Point(3, 1), Point(5, 1)) is Near to Point(2, 0), Point(3, 0), Point(4, 0), Point(3, 2), Point(6, 3)
            point.X.Between(X - 1, X + 1) && point.Y.Between(StartY - 1, EndY + 1);
    }

    private sealed record Symbol(string Value, Point Position);
}
