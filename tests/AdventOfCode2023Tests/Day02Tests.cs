namespace AdventOfCode2023Tests;

using adventofcode2023.Day02;

public class Day02Tests
{
    [Theory]
    [InlineData("Day02/example.txt", 8)]
    [InlineData("Day02/input.txt", 2447)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day02.Part1(file));

    [Theory]
    [InlineData("Day02/example.txt", 2286)]
    [InlineData("Day02/input.txt", 56322)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day02.Part2(file));
}
