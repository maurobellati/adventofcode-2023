namespace adventofcode2023.Day10;

using System.Collections;

public static class Day10
{
    public static int Part1(string file)
    {
        var lines = File.ReadAllLines(file);
        var grid = new Grid(lines);
        var start = FindStart(lines);
        var loop = FindLoop(grid, start);

        return loop.Count / 2;
    }

    public static int Part2(string file)
    {
        var lines = File.ReadAllLines(file);
        var grid = new Grid(lines);
        var start = FindStart(lines);
        var loop = FindLoop(grid, start);

        // double the size of the grid, so the flood fill can go between contiguous segments
        var box = grid.Box * 2;

        // double the resolution of the loop, so there are no gaps between loop segments
        var tightLoop = loop
            .Select(segment => segment with { Cell = segment.Cell * 2 })
            .SelectMany(
                segment => new[]
                {
                    segment.Cell,
                    segment.NextCell()
                })
            .ToHashSet();

        HashSet<Cell> visited = [..tightLoop];
        HashSet<Cell> inside = [];
        foreach (var cell in box.Where(it => !visited.Contains(it)))
        {
            var (floodedCells, isInside) = Flood(cell, tightLoop, box);
            visited.UnionWith(floodedCells);
            if (isInside)
            {
                // select only the flooded cells that are on the main grid (the ones that have even coordinates)
                var mainGridFloodedCells = floodedCells.Where(it => it.Row % 2 == 0 && it.Col % 2 == 0).ToHashSet();
                inside.UnionWith(mainGridFloodedCells);
            }
        }

        return inside.Count;
    }

    private static List<Segment> FindLoop(Grid grid, Cell start)
    {
        // find the first segment that is not a dead end
        var startSegment = Enum.GetValues<Direction>()
            .Select(direction => new Segment(start, 'S', direction))
            .First(segment => NextSegment(grid, segment) != null);

        List<Segment> loop = [startSegment];

        var current = startSegment;
        do
        {
            current = NextSegment(grid, current);
            loop.Add(current ?? throw new InvalidOperationException($"the loop is not closed: [{string.Join(", ", loop)}]"));
        } while (current.NextCell() != startSegment.Cell);

        return loop;
    }

    private static Cell FindStart(string[] lines) =>
        Enumerable.Range(0, lines.Length)
            .Select(row => new Cell(row, lines[row].IndexOf('S')))
            .Single(cell => cell.Col >= 0);

    private static (ISet<Cell> Visited, bool IsInside) Flood(Cell cell, ISet<Cell> loop, Box box)
    {
        HashSet<Cell> visited = [cell];
        var queue = new Queue<Cell>([cell]);
        var isInside = true;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var direction in Enum.GetValues<Direction>())
            {
                var next = current.Step(Directions.Steps[direction]);
                if (visited.Contains(next) || loop.Contains(next))
                {
                    continue;
                }

                if (!box.Contains(next))
                {
                    isInside = false;
                    continue;
                }

                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        return (visited, isInside);
    }

    private static Segment? NextSegment(Grid grid, Segment input)
    {
        var next = input.NextCell();
        if (!grid.Box.Contains(next))
        {
            return null;
        }

        var symbol = grid.GetSymbolAt(next);
        var pipeType = PipeType.FromSymbol(symbol);
        if (pipeType is null)
        {
            return null;
        }

        var inDirection = Directions.Opposites[input.Direction];
        if (!pipeType.CanConnectFrom(inDirection))
        {
            return null;
        }

        return new(next, symbol, pipeType.GetOutDirection(inDirection));
    }

    private sealed record Cell(int Row, int Col)
    {
        public static Cell operator *(Cell cell, int factor) => new(cell.Row * factor, cell.Col * factor);

        public Cell Step(Cell step) => new(Row + step.Row, Col + step.Col);
    }

    private enum Direction
    {
        N,
        S,
        E,
        W
    }

    private static class Directions
    {
        public static readonly Dictionary<Direction, Cell> Steps = new()
        {
            { Direction.N, new(-1, 0) },
            { Direction.S, new(1, 0) },
            { Direction.E, new(0, 1) },
            { Direction.W, new(0, -1) }
        };

        public static readonly Dictionary<Direction, Direction> Opposites = new()
        {
            { Direction.N, Direction.S },
            { Direction.S, Direction.N },
            { Direction.E, Direction.W },
            { Direction.W, Direction.E }
        };
    }

    private sealed record PipeType(char Symbol, Direction[] Directions)
    {
        private static readonly Dictionary<char, PipeType> All = new PipeType[]
        {
            new('|', [Direction.N, Direction.S]),
            new('-', [Direction.E, Direction.W]),
            new('L', [Direction.N, Direction.E]),
            new('J', [Direction.N, Direction.W]),
            new('7', [Direction.S, Direction.W]),
            new('F', [Direction.S, Direction.E])
        }.ToDictionary(it => it.Symbol, it => it);

        public static PipeType? FromSymbol(char symbol)
        {
            All.TryGetValue(symbol, out var result);
            return result;
        }

        public bool CanConnectFrom(Direction direction) => Directions.Contains(direction);

        public Direction GetOutDirection(Direction inDirection) => Directions.Single(d => d != inDirection);
    }

    private sealed record Segment(Cell Cell, char Symbol, Direction Direction)
    {
        public Cell NextCell() => Cell.Step(Directions.Steps[Direction]);
    }

    private sealed record Box(Cell TopLeft, Cell BottomRight) : IEnumerable<Cell>
    {
        public IEnumerator<Cell> GetEnumerator()
        {
            for (var row = TopLeft.Row; row <= BottomRight.Row; row++)
            {
                for (var col = TopLeft.Col; col <= BottomRight.Col; col++)
                {
                    yield return new(row, col);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Box operator *(Box box, int factor) => new(box.TopLeft * factor, box.BottomRight * factor);

        public bool Contains(Cell cell) => cell.Row.Between(TopLeft.Row, BottomRight.Row) && cell.Col.Between(TopLeft.Col, BottomRight.Col);
    }

    private sealed class Grid
    {
        private readonly string[] lines;

        public Grid(string[] lines)
        {
            this.lines = lines;
            Box = new(new(0, 0), new(lines.Length - 1, lines[0].Length - 1));
        }

        public Box Box { get; }

        public char GetSymbolAt(Cell next) => lines[next.Row][next.Col];
    }
}
