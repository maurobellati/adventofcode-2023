namespace AdventOfCode2023Tests;

using adventofcode2023.Day10;

public class Day10Tests
{
    [Theory]
    [InlineData("Day10/square-loop.txt", 4)]
    [InlineData("Day10/complex-loop.txt", 8)]
    [InlineData("Day10/input.txt", 6820)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day10.Part1(file));

    [Theory]
    [InlineData("Day10/tiles-4-a.txt", 4)]
    [InlineData("Day10/tiles-4-b.txt", 4)]
    [InlineData("Day10/tiles-10-junk.txt", 10)]
    [InlineData("Day10/input.txt", 337)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day10.Part2(file));
}
