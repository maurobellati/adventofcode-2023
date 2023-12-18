namespace AdventOfCode2023Tests;

using adventofcode2023.Day15;

public class Day15Tests
{
    [Theory]
    [InlineData("Day15/example.txt", 1320)]
    [InlineData("Day15/input.txt", 502139)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day15.Part1(file));

    [Theory]
    [InlineData("Day15/example.txt", 145)]
    [InlineData("Day15/input.txt", 284132)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day15.Part2(file));
}
