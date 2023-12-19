namespace AdventOfCode2023Tests;

using adventofcode2023.Day13;

public class Day13Tests
{
    [Theory]
    [InlineData("Day13/example.txt", 405)]
    [InlineData("Day13/input.txt", 27505)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day13.Part1(file));

    // [Theory]
    // [InlineData("Day13/example.txt", 0)]
    // [InlineData("Day13/input.txt", 0)]
    // public void Part2(string file, int expected) => Assert.Equal(expected, Day13.Part2(file));
}
