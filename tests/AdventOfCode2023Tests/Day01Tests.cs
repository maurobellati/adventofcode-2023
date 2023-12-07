namespace AdventOfCode2023Tests;

using adventofcode2023.Day01;

public class Day01Tests
{
    [Theory]
    [InlineData("Day01/example-01.txt", 142)]
    [InlineData("Day01/input.txt", 56042)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day01.Part1(file));

    [Theory]
    [InlineData("Day01/example-02.txt", 281)]
    [InlineData("Day01/input.txt", 55358)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day01.Part2(file));
}
