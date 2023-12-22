namespace adventofcode2023.Day17;

public static class Day17
{
    public static int Part1(string file) => Solve(file, (0, 3));

    public static int Part2(string file) => Solve(file, (4, 10));

    private static Node Dijkstra(Grid grid, Cell start, Cell goal, (int Min, int Max) sameDirectionMinMaxSteps)
    {
        var open = new HashSet<Node>();
        var processed = new HashSet<Node>();

        var startNode = new Node(start, null, 0, null);

        open.Add(startNode);
        while (open.Count > 0)
        {
            var current = open.MinBy(it => it.Cost)!;
            open.Remove(current);
            // Console.WriteLine("\nProcessing {0}", current);

            if (current.Cell == goal && current.History?.Count >= sameDirectionMinMaxSteps.Min)
            {
                Console.WriteLine("Found goal! Cost={0}. Processed {1} nodes", current.Cost, processed.Count);
                return current;
            }

            foreach (var direction in GetAllowedDirections(current, sameDirectionMinMaxSteps))
            {
                var neighborCell = current.Cell.Step(direction);
                if (!grid.IsInside(neighborCell))
                {
                    continue;
                }

                var history = current.History?.Direction == direction ? current.History with { Count = current.History.Count + 1 } : new(direction, 1);
                var neighborNode = new Node(
                    neighborCell,
                    history,
                    current.Cost + grid.Cost(neighborCell),
                    current);
                if (processed.Contains(neighborNode))
                {
                    // skip already processed nodes
                    continue;
                }

                // Console.WriteLine("  Adding {0} to open list", neighborNode);
                open.Add(neighborNode);
            }

            // mark current node as processed, after all neighbors have been visited
            processed.Add(current);
        }

        throw new InvalidOperationException("No path found");
    }

    private static IEnumerable<Direction> GetAllowedDirections(Node current, (int Min, int Max) sameDirectionMinMaxSteps)
    {
        if (current.History is null)
        {
            return Direction.All;
        }

        return Direction.All
            .Where(d => !d.IsOppositeOf(current.History.Direction)) // disallow going back
            .Where(d => d == current.History.Direction || current.History.Count >= sameDirectionMinMaxSteps.Min) // disallow short runs in same direction
            .Where(d => d != current.History.Direction || current.History.Count < sameDirectionMinMaxSteps.Max); // disallow long runs in same direction
    }

    private static List<Node> GetPath(Node result)
    {
        var path = new List<Node>();
        var current = result;
        while (current != null)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private static int Solve(string file, (int, int) minMaxConsecutiveStepsInSameDirection)
    {
        var lines = File.ReadAllLines(file);
        var grid = new Grid(lines.Select(line => line.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray());
        var start = new Cell(0, 0);
        var goal = new Cell(grid.Rows - 1, grid.Cols - 1);

        var result = Dijkstra(grid, start, goal, minMaxConsecutiveStepsInSameDirection);
        GetPath(result).ForEach(Console.WriteLine);

        return result.Cost;
    }

    public record Direction(int DeltaRow, int DeltaCol)
    {
        public static readonly Direction N = new(-1, 0);
        public static readonly Direction S = new(1, 0);
        public static readonly Direction E = new(0, 1);
        public static readonly Direction W = new(0, -1);

        public static readonly IReadOnlyList<Direction> All = [N, S, E, W];

        public bool IsOppositeOf(Direction other) => DeltaRow == -other.DeltaRow && DeltaCol == -other.DeltaCol;
    }

    public sealed class Node(Cell cell, DirectionHistory? history, int cost, Node? parent) : IEquatable<Node>
    {
        public Cell Cell { get; } = cell;

        public int Cost { get; } = cost;

        public DirectionHistory? History { get; } = history;

        public Node? Parent { get; } = parent;

        public bool Equals(Node? other) => other != null && Cell.Equals(other.Cell) && Equals(History, other.History);

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || (obj is Node other && Equals(other));

        public override int GetHashCode() => HashCode.Combine(Cell, History);

        public override string ToString() => $"Node({Cell}, {History}, Cost={Cost})";
    }

    public record DirectionHistory(Direction Direction, int Count);

    public record Cell(int Row, int Col)
    {
        public Cell Step(Direction direction) => new(Row + direction.DeltaRow, Col + direction.DeltaCol);
    }

    public class Grid
    {
        public Grid(int[][] costs)
        {
            Cols = costs[0].Length;
            Rows = costs.Length;
            Costs = costs;
        }

        public int Cols { get; }

        public int[][] Costs { get; }

        public int Rows { get; }

        public int Cost(Cell cell) => Costs[cell.Row][cell.Col];

        public bool IsInside(Cell cell) => cell.Row >= 0 && cell.Row < Rows && cell.Col >= 0 && cell.Col < Cols;
    }
}
