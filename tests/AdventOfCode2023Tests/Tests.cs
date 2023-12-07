namespace AdventOfCode2023Tests;

using adventofcode2023.Day01;
using adventofcode2023.Day02;

public class Tests
{
    [Theory]
    [InlineData("Day01/example-01.txt", 142)]
    [InlineData("Day01/input.txt", 56042)]
    public void Day01Part1(string file, int expected) => Assert.Equal(expected, Day01.Part1(file));

    [Theory]
    [InlineData("Day01/example-02.txt", 281)]
    [InlineData("Day01/input.txt", 55358)]
    public void Day01Part2(string file, int expected) => Assert.Equal(expected, Day01.Part2(file));

    [Theory]
    [InlineData("Day02/example.txt", 8)]
    [InlineData("Day02/input.txt", 2447)]
    public void Day02Part1(string file, int expected) => Assert.Equal(expected, Day02.Part1(file));

    [Theory]
    [InlineData("Day02/example.txt", 2286)]
    [InlineData("Day02/input.txt", 56322)]
    public void Day02Part2(string file, int expected) => Assert.Equal(expected, Day02.Part2(file));
}
