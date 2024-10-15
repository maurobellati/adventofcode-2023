namespace AdventOfCode2023Tests;

using adventofcode2023.Day24;

public class Day24Tests
{
    [Theory]
    [InlineData("Day24/example.txt", 7, 27, 2)]
    [InlineData("Day24/input.txt", 200000000000000, 400000000000000, 17244)]
    public void Part1(string file, long min, long max, long expected) => Assert.Equal(expected, Day24.Part1(file, min, max));

    [Theory]
    [InlineData("Day24/example.txt", 47)]
    [InlineData("Day24/input.txt", 0)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day24.Part2(file));
}
