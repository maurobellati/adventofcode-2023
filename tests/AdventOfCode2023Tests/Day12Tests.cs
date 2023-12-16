namespace AdventOfCode2023Tests;

using adventofcode2023.Day12;
using FluentAssertions;

public class Day12Tests
{
    [Theory]
    [InlineData("???.### 1,1,3", 1)]
    [InlineData(".??..??...?##. 1,1,3", 4)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [InlineData("????.#...#... 4,1,1", 1)]
    [InlineData("????.######..#####. 1,6,5", 4)]
    [InlineData("?###???????? 3,2,1", 10)]
    public void Arrangements(string input, int expected)
    {
        var (record, groups) = Day12.Parse(input, 1);
        Day12.Arrangements(record, groups).Should().Be(expected);
    }

    [Theory]
    [InlineData("Day12/example.txt", 21)]
    [InlineData("Day12/input.txt", 7221)]
    public void Part1(string file, long expected) => Assert.Equal(expected, Day12.Part1(file));

    [Theory]
    [InlineData("Day12/example.txt", 525152)]
    [InlineData("Day12/input.txt", 7139671893722)]
    public void Part2(string file, long expected) => Assert.Equal(expected, Day12.Part2(file));

    [Theory]
    [InlineData("???.### 1,1,3", 1)]
    [InlineData(".??..??...?##. 1,1,3", 16384)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [InlineData("????.#...#... 4,1,1", 16)]
    [InlineData("????.######..#####. 1,6,5", 2500)]
    [InlineData("?###???????? 3,2,1", 506250)]
    public void UnfoldedArrangements(string input, int expected)
    {
        var (record, groups) = Day12.Parse(input, 5);
        Day12.Arrangements(record, groups).Should().Be(expected);
    }
}
