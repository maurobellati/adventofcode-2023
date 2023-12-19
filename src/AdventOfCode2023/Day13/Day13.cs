namespace adventofcode2023.Day13;

using CSharpFunctionalExtensions;

public static class Day13
{
    public static int Part1(string file) =>
        ParsePatterns(File.ReadAllLines(file))
            .Select(FindSummary)
            .Sum();

    public static int Part2(string file) => 0;

    private static Maybe<int> FindHorizontalSummary(Pattern pattern) =>
        FindHorizontalSymmetry(pattern).Map(it => it * 100);

    private static Maybe<int> FindHorizontalSymmetry(Pattern pattern)
    {
        // starting from line 0 to n-1, consider line n and n+!
        // if they are same, try n-1 and n+2 and so forth, until we find a different line or we reach the end

        for (var row = 0; row < pattern.RowCount - 1; row++)
        {
            if (pattern.Lines[row] == pattern.Lines[row + 1])
            {
                var i = 0;
                while (row - i >= 0 && row + 1 + i < pattern.RowCount)
                {
                    if (pattern.Lines[row - i] != pattern.Lines[row + 1 + i])
                    {
                        break;
                    }

                    i++;
                }

                if (row - i < 0 || row + 1 + i >= pattern.RowCount)
                {
                    // we reached the end, so we found a symmetric pattern
                    return row + 1;
                }
            }
        }

        return Maybe<int>.None;
    }

    private static int FindSummary(Pattern pattern) =>
        FindHorizontalSummary(pattern).Or(FindVerticalSummary(pattern)).GetValueOrThrow(new ArgumentException("No symmetry found"));

    private static Maybe<int> FindVerticalSummary(Pattern pattern) => FindVerticalSymmetry(pattern);

    private static Maybe<int> FindVerticalSymmetry(Pattern pattern)
    {
        // starting from column 0 to n-1, consider column n and n+1
        // if they are same, try n-1 and n+2 and so forth, until we find a different column or we reach the end
        for (var col = 0; col < pattern.ColCount - 1; col++)
        {
            if (pattern.Lines.TrueForAll(it => it[col] == it[col + 1]))
            {
                var i = 0;
                while (col - i >= 0 && col + 1 + i < pattern.ColCount)
                {
                    if (pattern.Lines.Any(it => it[col - i] != it[col + 1 + i]))
                    {
                        break;
                    }

                    i++;
                }

                if (col - i < 0 || col + 1 + i >= pattern.ColCount)
                {
                    // we reached the end, so we found a symmetric pattern
                    return col + 1;
                }
            }
        }

        return Maybe<int>.None;
    }

    private static List<Pattern> ParsePatterns(string[] lines)
    {
        // lines are in block of n patterns, separated by an empty line
        // example:
        // .##...#..##
        // .....##...#
        // #..#..###..
        //
        // .##...#..##
        // .....##...#
        // #..#..###..

        // parse each block into its pattern
        List<Pattern> patterns = [];

        List<string> currentPatternLines = [];
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                patterns.Add(new(currentPatternLines));
                currentPatternLines = new();
            }
            else
            {
                currentPatternLines.Add(line);
            }
        }

        patterns.Add(new(currentPatternLines));
        return patterns;
    }
}

internal class Pattern
{
    public Pattern(List<string> lines)
    {
        Lines = lines;
        ColCount = lines[0].Length;
        RowCount = lines.Count;
    }

    public int ColCount { get; }

    public List<string> Lines { get; }

    public int RowCount { get; }
}
