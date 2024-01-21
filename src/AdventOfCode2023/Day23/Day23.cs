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

    private static readonly Dictionary<Direction, char> DirToSlope = new()
    {
        [Direction.U] = SlopeUp,
        [Direction.D] = SlopeDown,
        [Direction.R] = SlopeRight,
        [Direction.L] = SlopeLeft
    };

    private static Grid grid;
    private static Cell goal;

    public static int Part1(string file) => SolveWith(file, BuildGetCandidates(true));

    public static int Part2(string file) => SolveWith(file, BuildGetCandidates(false));

    private static Func<Cell, Path, Grid, IEnumerable<Cell>> BuildGetCandidates(bool considerSlope) =>
        (cell, path, grid) => Direction.All
            .Select(
                direction => new
                {
                    Direction = direction,
                    Candidate = cell.Move(direction)
                })
            .Where(move => grid.Contains(move.Candidate))
            .Where(move => grid.ValueAt(move.Candidate) != Forest)
            .Where(move => !considerSlope || grid.ValueAt(move.Candidate) == Open || grid.ValueAt(move.Candidate) == DirToSlope[move.Direction])
            .Where(move => !path.Contains(move.Candidate))
            .Select(move => move.Candidate);

    private static void DfsIterative(
        Func<Cell, Path, Grid, IEnumerable<Cell>> getCandidates,
        Action<Path> processSolution,
        Cell start)
    {
        // path is the full path from start to goal
        Path path = new([start]);

        Stack<Cell> choices = new([start]);

        // forks contains just the nodes where there is a choice
        Stack<Cell> forks = new([start]);


        while (choices.Any())
        {
            var currentCell = choices.Pop();
            path.Push(currentCell);

            if (currentCell == goal)
            {
                processSolution(path);
                while (path.Peek() != forks.Peek())
                {
                    path.Pop();
                }
            }

            var candidates = getCandidates(currentCell, path, grid).ToList();
            while (candidates.Count == 1)
            {
                currentCell = candidates[0];
                path.Push(currentCell);
                if (currentCell == goal)
                {
                    processSolution(path);
                    while (path.Peek() != forks.Peek())
                    {
                        path.Pop();
                    }
                }

                candidates = getCandidates(currentCell, path, grid).ToList();
            }

            forks.Push(currentCell);
            foreach (var candidate in candidates)
            {
                choices.Push(candidate);
            }
        }
    }

    private static Cell FindOpenCell(string[] lines, int row) => new(row, lines[row].IndexOf(Open));

    private static int SolveWith(string file, Func<Cell, Path, Grid, IEnumerable<Cell>> getCandidates)
    {
        var lines = File.ReadAllLines(file);
        grid = new(lines);
        var start = FindOpenCell(lines, 0);
        goal = FindOpenCell(lines, lines.Length - 1);

        Path path = new([start]);

        List<int> solutionLengths = [];
        Action<Path> goalReached = solution =>
        {
            solutionLengths.Add(solution.Count - 1);
            // Console.WriteLine($"Goal reached in {solution.Count} steps");
            // Console.WriteLine($"Path: {string.Join(" -> ", solution)}");
        };

        // DfsRecursive(getCandidates, goalReached, path);
        DfsIterative(getCandidates, goalReached, start);

        return solutionLengths.Max();
    }

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

    private sealed class Grid
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
