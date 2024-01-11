namespace AdventOfCode2023Tests;

using adventofcode2023.Day22;

public class Day22Tests
{
    [Theory]
    [InlineData("Day22/example.txt", 5)]
    [InlineData("Day22/input.txt", 424)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day22.Part1(file));

    [Theory]
    [InlineData("Day22/example.txt", 7)]
    [InlineData("Day22/input.txt", 55483)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day22.Part2(file));
}
