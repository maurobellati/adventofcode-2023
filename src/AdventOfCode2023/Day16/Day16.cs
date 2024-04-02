#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
namespace adventofcode2023.Day16;

using CSharpFunctionalExtensions;

public static class Day16
{
    public enum Direction
    {
        N,
        E,
        S,
        W
    }

    public static int Part1(string file)
    {
        var grid = new Grid(File.ReadAllLines(file));
        return CountEnergizedCells(new(0, 0, Direction.E), grid);
    }

    public static int Part2(string file)
    {
        var grid = new Grid(File.ReadAllLines(file));

        // consider all possible origins: for each cell in the border (all verticals, all horizontals)
        var origins = new List<BeamSegment>();
        for (var row = 0; row < grid.Rows; row++)
        {
            origins.Add(new(row, 0, Direction.E));
            origins.Add(new(row, grid.Cols - 1, Direction.W));
        }

        for (var col = 0; col < grid.Cols; col++)
        {
            origins.Add(new(0, col, Direction.S));
            origins.Add(new(grid.Rows - 1, col, Direction.N));
        }

        return origins.Select(it => CountEnergizedCells(it, grid)).Max();
    }

    private static int CountEnergizedCells(BeamSegment origin, Grid grid)
    {
        var first = grid.Hit(origin.Cell, origin.Directions[0]);
        HashSet<BeamSegment> visited = [];
        var queue = new Queue<BeamSegment>([first]);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current);

            foreach (var direction in current.Directions)
            {
                // Console.Write("Exploring {0} in direction {1}", current, direction);
                var nextCell = current.Cell.Step(direction);
                if (grid.IsInside(nextCell))
                {
                    var next = grid.Hit(nextCell, direction);
                    if (visited.Add(next))
                    {
                        // Console.WriteLine(" -> {0} enqueued", next);
                        queue.Enqueue(next);
                    }
                    // Console.WriteLine(" -> {0} already visited", next);
                }
                // Console.WriteLine($" -> hit the edge at {nextCell}");
            }
        }

        return visited.Select(it => it.Cell).Distinct().Count();
    }

    public record Cell(int Row, int Col)
    {
        public override string ToString() => $"Cell({Row}, {Col})";

        public Cell Step(Direction direction) => direction switch
        {
            Direction.N => this with { Row = Row - 1 },
            Direction.E => this with { Col = Col + 1 },
            Direction.S => this with { Row = Row + 1 },
            Direction.W => this with { Col = Col - 1 }
        };
    }

    public class BeamSegment(Cell cell, List<Direction> directions) : ValueObject
    {
        public BeamSegment(int row, int col, Direction direction) : this(new(row, col), [direction])
        {
        }

        public Cell Cell { get; } = cell;

        public List<Direction> Directions { get; } = directions;

        public override string ToString() => $"BeamSegment({Cell}, [{string.Join(",", Directions)}])";

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Cell.Row;
            yield return Cell.Col;
            foreach (var direction in Directions)
            {
                yield return direction;
            }
        }
    }

    public class Grid(string[] lines)
    {
        public int Cols { get; } = lines[0].Length;

        private string[] Lines { get; } = lines;

        public int Rows { get; } = lines.Length;

        public BeamSegment Hit(Cell target, Direction direction)
        {
            var symbol = Lines[target.Row][target.Col];
            List<Direction> newDirections = symbol switch
            {
                '.' => [direction],
                '/' =>
                    direction switch
                    {
                        Direction.N => [Direction.E],
                        Direction.E => [Direction.N],
                        Direction.S => [Direction.W],
                        Direction.W => [Direction.S]
                    },
                '\\' =>
                    direction switch
                    {
                        Direction.N => [Direction.W],
                        Direction.E => [Direction.S],
                        Direction.S => [Direction.E],
                        Direction.W => [Direction.N]
                    },
                '|' =>
                    direction switch
                    {
                        Direction.N => [Direction.N],
                        Direction.S => [Direction.S],
                        Direction.E or Direction.W => [Direction.N, Direction.S]
                    },
                '-' =>
                    direction switch
                    {
                        Direction.E => [Direction.E],
                        Direction.W => [Direction.W],
                        Direction.N or Direction.S => [Direction.W, Direction.E]
                    },
                _ => throw new ArgumentException($"Unknown symbol {symbol}")
            };
            return new(target, newDirections);
        }

        public bool IsInside(Cell cell) => cell.Row >= 0 && cell.Row < Rows && cell.Col >= 0 && cell.Col < Cols;
    }
}
