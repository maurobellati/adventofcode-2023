namespace AdventOfCode2023Tests;

using adventofcode2023.Day17;

public class Day17Tests
{
    [Theory]
    [InlineData("Day17/example.txt", 102)]
    [InlineData("Day17/input.txt", 1065)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day17.Part1(file));

    [Theory]
    [InlineData("Day17/example.txt", 94)]
    [InlineData("Day17/input.txt", 1249)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day17.Part2(file));
}
