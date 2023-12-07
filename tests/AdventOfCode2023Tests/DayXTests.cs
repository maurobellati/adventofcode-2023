namespace AdventOfCode2023Tests;

using adventofcode2023.DayX;

public class DayXTests
{
    [Theory]
    [InlineData("DayX/example.txt", 0)]
    [InlineData("DayX/input.txt", 0)]
    public void Part1(string file, int expected) => Assert.Equal(expected, DayX.Part1(file));

    [Theory]
    [InlineData("DayX/example.txt", 0)]
    [InlineData("DayX/input.txt", 0)]
    public void Part2(string file, int expected) => Assert.Equal(expected, DayX.Part2(file));
}
