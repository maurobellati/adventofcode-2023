namespace adventofcode2023.Day21;

using System.Diagnostics;

public static class Day21
{
    private const int Rock = '#';

    /// <summary>
    ///     Computes a mod b.
    ///     This behaves differently than C# % operator that is a remainder, NOT a modulo.
    ///     % operator returns a negative number when input is negative.
    /// </summary>
    /// <remarks>See: https://torstencurdt.com/tech/posts/modulo-of-negative-numbers/</remarks>
    /// <returns>a mod b also for negative values of a. The return value is always between 0 and b</returns>
    public static int Mod(int a, int b) => (a % b + b) % b;

    public static int Part1(string file, int steps)
    {
        var lines = File.ReadAllLines(file);
        var grid = ParseGrid(lines);
        var start = FindStart(lines);

        return Bfs(grid, start, steps);
    }

    public static long Part2Quadratic(string file, int steps)
    {
        var lines = File.ReadAllLines(file);
        var grid = ParseGrid(lines);
        var start = FindStart(lines);

        Debug.Assert(grid.Rows == grid.Cols, "grid must be a square");
        var size = grid.Rows;
        Debug.Assert(size % 2 == 1, "size of the grid must be odd");
        var halfSize = (grid.Rows - 1) / 2;

        // the steps grow with quadratic rate. We can use three points to compute the quadratic function
        // and then use the function to compute the number of steps for the input value
        var p1 = Bfs(grid, start, halfSize);
        var p2 = Bfs(grid, start, size + halfSize);
        var p3 = Bfs(grid, start, 2 * size + halfSize);

        var n = (steps * 1L - halfSize) / size;
        var a = (p3 - 2 * p2 + p1) / 2;
        var b = p2 - p1 - a;
        var c = p1;
        return c + b * n + a * n * n;
    }

    public static long Part2SimpleBfs(string file, int steps) =>
        // same as part 1
        Part1(file, steps);

    private static int Bfs(Grid grid, Cell start, int steps)
    {
        // store the frontier for each step
        List<HashSet<Cell>> frontierByStep =
        [
            [start]
        ];

        // store ALL visited cells to avoid visiting them again
        HashSet<Cell> visited = [start];

        for (var i = 1; i <= steps; i++)
        {
            // use the frontier from the previous step
            var frontier = frontierByStep[i - 1];

            HashSet<Cell> discovered = [];
            foreach (var candidate in frontier.SelectMany(
                             it => new[]
                             {
                                 it.Move(Direction.U),
                                 it.Move(Direction.D),
                                 it.Move(Direction.L),
                                 it.Move(Direction.R)
                             })
                         .Where(it => grid.ValueAt(it) != Rock))
            {
                if (!visited.Add(candidate))
                {
                    // Console.WriteLine($"Cell {candidate} already visited");
                    continue;
                }

                // Console.WriteLine($"Cell {candidate} added to visited");
                discovered.Add(candidate);
            }

            frontierByStep.Add(discovered);
        }

        // sum the frontier for each step, taking just the cells at the same parity of the input steps
        var result = frontierByStep.Where((_, step) => step % 2 == steps % 2).Sum(it => it.Count);
        Console.WriteLine($"steps: {steps}, result: {result}");
        return result;
    }

    private static Cell FindStart(string[] lines) =>
        Enumerable.Range(0, lines.Length)
            .Select(row => new Cell(row, lines[row].IndexOf('S')))
            .Single(cell => cell.Col >= 0);

    private static Grid ParseGrid(string[] lines) => new(lines);

    internal class Grid
    {
        private readonly string[] lines;

        internal Grid(string[] lines)
        {
            this.lines = lines;
            Rows = lines.Length;
            Cols = lines[0].Length;
        }

        public int Cols { get; }

        public int Rows { get; }

        public char ValueAt(Cell cell) => lines[Mod(cell.Row, Rows)][Mod(cell.Col, Cols)];
    }

    public record Direction(int DeltaX, int DeltaY)
    {
        public static readonly Direction U = new(-1, 0);
        public static readonly Direction D = new(1, 0);
        public static readonly Direction L = new(0, -1);
        public static readonly Direction R = new(0, 1);
    }

    internal record Cell(int Row, int Col)
    {
        public override string ToString() => $"({Row}, {Col})";

        public Cell Move(Direction direction) => new(Row + direction.DeltaX, Col + direction.DeltaY);
    }
}
