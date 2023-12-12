namespace AdventOfCode2023Tests;

using adventofcode2023.Day08;

public class Day08Tests
{
    [Theory]
    [InlineData("Day08/example-01.txt", 2)]
    [InlineData("Day08/example-02.txt", 6)]
    [InlineData("Day08/input.txt", 23147)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day08.Part1(file));

    [Theory]
    [InlineData("Day08/example-03.txt", 6)]
    [InlineData("Day08/input.txt", 22289513667691L)]
    public void Part2(string file, long expected) => Assert.Equal(expected, Day08.Part2(file));
}
