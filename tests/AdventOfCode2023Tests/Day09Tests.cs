namespace AdventOfCode2023Tests;

using adventofcode2023.Day09;

public class Day09Tests
{
    [Theory]
    [InlineData("Day09/example.txt", 114)]
    [InlineData("Day09/input.txt", 1877825184)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day09.Part1(file));

    [Theory]
    [InlineData("Day09/example.txt", 2)]
    [InlineData("Day09/input.txt", 1108)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day09.Part2(file));
}
