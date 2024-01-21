namespace adventofcode2023.Day23;

using Path = Stack<Day23.Cell>;

public static class Day23
{
    private static readonly char Forest = '#';
    private static readonly char Open = '.';
    private static readonly char SlopeUp = '^';
    private static readonly char SlopeDown = 'v';
    private static readonly char SlopeRight = '>';
    private static readonly char SlopeLeft = '<';

    private static readonly HashSet<(Direction, char)> SlopeCompatibility =
    [
        (Direction.U, SlopeUp),
        (Direction.D, SlopeDown),
        (Direction.R, SlopeRight),
        (Direction.L, SlopeLeft)
    ];

    private static readonly Dictionary<Direction, char> DirToSlope = new()
    {
        [Direction.U] = SlopeUp,
        [Direction.D] = SlopeDown,
        [Direction.R] = SlopeRight,
        [Direction.L] = SlopeLeft
    };

    private static Grid grid;
    private static Cell goal;

    public static int Part1(string file)
    {
        var lines = File.ReadAllLines(file);
        grid = new(lines);
        var start = FindOpenCell(lines, 0);
        goal = FindOpenCell(lines, lines.Length - 1);

        Path path = new([start]);

        List<Path> solutions = [];
        Action<Path> goalReached = solution =>
        {
            solutions.Add(new([..solution]));
            Console.WriteLine($"Goal reached in {solution.Count} steps");
            // Console.WriteLine($"Path: {string.Join(" -> ", solution)}");
        };

        Dfs(goalReached, path);

        return solutions.MaxBy(it => it.Count)!.Count - 1;
    }

    public static int Part2(string file) => 0;

    private static void Dfs(Action<Path> processSolution, Path path)
    {
        var currentCell = path.Peek();

        if (currentCell == goal)
        {
            processSolution(path);
            return;
        }

        foreach (var direction in Direction.All)
        {
            var candidate = currentCell.Move(direction);
            if (!grid.Contains(candidate))
            {
                continue;
            }

            if (!IsCellValid(candidate, direction))
            {
                continue;
            }

            if (path.Contains(candidate))
            {
                continue;
            }

            path.Push(candidate);
            Dfs(processSolution, path);
            path.Pop();
        }
    }

    private static Cell FindOpenCell(string[] lines, int row) => new(row, lines[row].IndexOf(Open));

    private static bool IsCellValid(Cell cell, Direction direction)
    {
        var cellValue = grid.ValueAt(cell);
        return cellValue == Open || cellValue == DirToSlope[direction];
    }

    private static bool IsSlopeValid(char cellValue, Direction direction) =>
        DirToSlope[direction] == cellValue;
    // SlopeCompatibility.Contains((direction, cellValue));

    internal readonly record struct Direction(int DeltaX, int DeltaY)
    {
        public static readonly Direction U = new(-1, 0);
        public static readonly Direction D = new(1, 0);
        public static readonly Direction L = new(0, -1);
        public static readonly Direction R = new(0, 1);
        public static readonly List<Direction> All = [U, D, L, R];
    }

    internal readonly record struct Cell(int Row, int Col)
    {
        public override string ToString() => $"({Row}, {Col})";

        public Cell Move(Direction direction) => new(Row + direction.DeltaX, Col + direction.DeltaY);
    }

    private class Grid
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

        public bool Contains(Cell cell) => cell.Row >= 0 && cell.Row < Rows && cell.Col >= 0 && cell.Col < Cols;

        public char ValueAt(Cell cell) => lines[cell.Row][cell.Col];
    }
}
