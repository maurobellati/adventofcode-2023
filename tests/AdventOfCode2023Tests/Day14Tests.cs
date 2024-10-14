namespace AdventOfCode2023Tests;

using adventofcode2023.Day14;
using FluentAssertions;

public class Day14Tests
{
    [Theory]
    [InlineData(
        1,
        """
        .....#....
        ....#...O#
        ...OO##...
        .OO#......
        .....OOO#.
        .O#...O#.#
        ....O#....
        ......OOOO
        #...O###..
        #..OO#....
        """)]
    [InlineData(
        2,
        """
        .....#....
        ....#...O#
        .....##...
        ..O#......
        .....OOO#.
        .O#...O#.#
        ....O#...O
        .......OOO
        #..OO###..
        #.OOO#...O
        """)]
    [InlineData(
        3,
        """
        .....#....
        ....#...O#
        .....##...
        ..O#......
        .....OOO#.
        .O#...O#.#
        ....O#...O
        .......OOO
        #...O###.O
        #.OOO#...O
        """)]
    public void CycleTest(int cycleCount, string expectedValue)
    {
        var platform = Day14.Platform.Parse(
            """
            O....#....
            O.OO#....#
            .....##...
            OO.#O....O
            .O.....O#.
            O.#..O.#.#
            ..O..#O..O
            .......O..
            #....###..
            #OO..#....
            """);

        for (var i = 0; i < cycleCount; i++)
        {
            platform.Cycle();
        }

        platform.Items.Order().Should()
            .BeEquivalentTo(Day14.Platform.Parse(expectedValue).Items.Order());
    }

    [Theory]
    [InlineData("Day14/example.txt", 136)]
    [InlineData("Day14/input.txt", 108813)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day14.Part1(file));

    [Theory]
    [InlineData("Day14/example.txt", 64)]
    [InlineData("Day14/input.txt", 104533)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day14.Part2(file));

    [Fact]
    public void TiltNorthTest()
    {
        var platform = Day14.Platform.Parse(
            """
            O....#....
            O.OO#....#
            .....##...
            OO.#O....O
            .O.....O#.
            O.#..O.#.#
            ..O..#O..O
            .......O..
            #....###..
            #OO..#....
            """);

        platform.TiltNorth();

        var expected = Day14.Platform.Parse(
            """
            OOOO.#.O..
            OO..#....#
            OO..O##..O
            O..#.OO...
            ........#.
            ..#....#.#
            ..O..#.O.O
            ..O.......
            #....###..
            #....#....
            """);

        platform.Should().Be(expected);
    }
}
