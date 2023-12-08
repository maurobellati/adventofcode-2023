namespace AdventOfCode2023Tests;

using adventofcode2023.Day06;

public class Day06Tests
{
    [Theory]
    [InlineData("Day06/example.txt", 288)]
    [InlineData("Day06/input.txt", 293046)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day06.Part1(file));

    [Theory]
    [InlineData("Day06/input.txt", 35150181)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day06.Part2(file));
}
