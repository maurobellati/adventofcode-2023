namespace adventofcode2023.Day10;

public static class Day10
{
    public static int Part1(string file)
    {
        var lines = File.ReadAllLines(file);
        var grid = Grid<char>.Create(lines);
        var start = FindStart(lines);
        var loop = FindLoop(grid, start);

        return loop.Count / 2;
    }

    public static int Part2(string file)
    {
        var lines = File.ReadAllLines(file);
        var grid = Grid<char>.Create(lines);
        var start = FindStart(lines);
        var loop = FindLoop(grid, start);

        // double the size of the grid, so the flood fill can go between contiguous segments
        var box = grid.GetBox() * 2;

        // double the resolution of the loop, so there are no gaps between loop segments
        var tightLoop = loop
            .Select(segment => segment with { Cell = segment.Cell * 2 })
            .SelectMany(
                segment => new[] { segment.Cell, segment.NextCell() })
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

    private static List<Segment> FindLoop(Grid<char> grid, Cell start)
    {
        // find the first segment that is not a dead end
        var startSegment = Direction.GetAll()
            .Select(direction => new Segment(start, 'S', direction))
            .First(segment => grid.NextSegment(segment) != null);

        List<Segment> loop = [startSegment];

        var current = startSegment;
        do
        {
            current = grid.NextSegment(current);
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
        HashSet<Cell> visitedCells = [cell];
        var floodFrontier = new Queue<Cell>([cell]);

        var isInside = true;
        while (floodFrontier.IsNotEmpty())
        {
            var currentCell = floodFrontier.Dequeue();
            foreach (var nextCell in currentCell.MoveMany(Direction.GetAll()))
            {
                if (visitedCells.Contains(nextCell) || loop.Contains(nextCell))
                {
                    continue;
                }

                isInside &= box.Contains(nextCell);
                if (!isInside)
                {
                    continue;
                }

                visitedCells.Add(nextCell);
                floodFrontier.Enqueue(nextCell);
            }
        }

        return (visitedCells, isInside);
    }

    public sealed record PipeType(char Symbol, Pair<Direction> Directions)
    {
        private static readonly Dictionary<char, PipeType> All = new PipeType[]
        {
            new('|', new(Direction.N, Direction.S)),
            new('-', new(Direction.E, Direction.W)),
            new('L', new(Direction.N, Direction.E)),
            new('J', new(Direction.N, Direction.W)),
            new('7', new(Direction.S, Direction.W)),
            new('F', new(Direction.S, Direction.E))
        }.ToDictionary(it => it.Symbol, it => it);

        public static PipeType? FromSymbol(char symbol)
        {
            All.TryGetValue(symbol, out var result);
            return result;
        }
    }

    public sealed record Segment(Cell Cell, char Symbol, Direction Direction)
    {
        public Cell NextCell() => Cell.Move(Direction);
    }
}

public static class GridExtensions
{
    public static Day10.Segment? NextSegment(this Grid<char> grid, Day10.Segment input)
    {
        var next = input.NextCell();
        if (!grid.Contains(next))
        {
            return null;
        }

        var symbol = grid.ValueAt(next);
        var pipeType = Day10.PipeType.FromSymbol(symbol);
        if (pipeType is null)
        {
            return null;
        }

        var inDirection = input.Direction.Opposite();
        if (!pipeType.Directions.Contains(inDirection))
        {
            return null;
        }

        return new(next, symbol, pipeType.Directions.Other(inDirection));
    }
}
