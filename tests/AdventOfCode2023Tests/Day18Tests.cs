namespace AdventOfCode2023Tests;

using adventofcode2023.Day18;

public class Day18Tests
{
    [Theory]
    [InlineData("Day18/example.txt", 62)]
    [InlineData("Day18/input.txt", 38188)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day18.Part1.Solve(file));

    [Theory]
    [InlineData("Day18/example.txt", 952408144115)]
    [InlineData("Day18/input.txt", 93325849869340)]
    public void Part2(string file, long expected) => Assert.Equal(expected, Day18.Part2.Solve(file));
}
