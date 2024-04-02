namespace adventofcode2023.Day17;

public static class Day17
{
    public static int Part1(string file) => Solve(file, (0, 3));

    public static int Part2(string file) => Solve(file, (4, 10));

    private static (Node Node, int Cost, Dictionary<Node, Node> Parents) Dijkstra(Grid grid, Cell start, Cell goal, (int Min, int Max) sameDirectionMinMaxSteps)
    {
        var queue = new PriorityQueue<(Node Node, int Cost), int>();
        var processed = new HashSet<Node>();
        var costs = new Dictionary<Node, int>();
        var parents = new Dictionary<Node, Node>();

        var startNode = new Node(start, null);
        queue.Enqueue((startNode, 0), 0);

        while (queue.Count > 0)
        {
            var (currentNode, currentCost) = queue.Dequeue();
            // Console.WriteLine("\nProcessing ({0}, {1}, Cost={2})", currentNode.Cell, currentNode.History, currentCost);

            if (currentNode.Cell == goal && currentNode.History?.Count >= sameDirectionMinMaxSteps.Min)
            {
                Console.WriteLine("Found goal! Cost={0}. Processed {1} nodes", currentCost, processed.Count);
                return (Node: currentNode, Cost: currentCost, parents);
            }

            foreach (var direction in GetAllowedDirections(currentNode, sameDirectionMinMaxSteps))
            {
                var nextCell = currentNode.Cell.Step(direction);
                if (!grid.IsInside(nextCell))
                {
                    continue;
                }

                var history = currentNode.History?.Direction == direction ? currentNode.History with { Count = currentNode.History.Count + 1 } : new(direction, 1);
                var nextNode = new Node(nextCell, history);
                if (processed.Contains(nextNode))
                {
                    // skip already processed nodes
                    // Console.WriteLine("  Skipping ({0}, {1})", nextNode.Cell, nextNode.History);
                    continue;
                }

                var nextCost = currentCost + grid.Costs[nextCell.Row][nextCell.Col];
                if (costs.TryGetValue(nextNode, out var bestCost) && bestCost <= nextCost)
                {
                    // skip nodes with same cell and history but higher cost
                    // Console.WriteLine("  Skipping ({0}, {1}, Cost={2}) because high cost", nextNode.Cell, nextNode.History, nextCost);
                    continue;
                }

                // Console.WriteLine("  Adding ({0}, {1}, Cost={2})", nextNode.Cell, nextNode.History, nextCost);
                queue.Enqueue((nextNode, nextCost), nextCost);
                parents[nextNode] = currentNode;
                costs[nextNode] = nextCost;
            }

            // mark current node as processed, after all neighbors have been visited
            processed.Add(currentNode);

            if (processed.Count % 100 == 0)
            {
                // Console.WriteLine("Processed {0} nodes", processedNodes.Count);
            }
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

    private static int Solve(string file, (int, int) minMaxConsecutiveStepsInSameDirection)
    {
        var lines = File.ReadAllLines(file);
        var grid = new Grid(lines.Select(line => line.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray());
        var start = new Cell(0, 0);
        var goal = new Cell(grid.Rows - 1, grid.Cols - 1);

        var result = Dijkstra(grid, start, goal, minMaxConsecutiveStepsInSameDirection);

        return result.Cost;
    }

    public record Direction(string Label, int DeltaRow, int DeltaCol)
    {
        private static readonly Direction N = new("N", -1, 0);
        private static readonly Direction S = new("S", 1, 0);
        private static readonly Direction E = new("E", 0, 1);
        private static readonly Direction W = new("W", 0, -1);

        public static readonly IReadOnlyList<Direction> All = [N, S, E, W];

        public override string ToString() => Label;

        public bool IsOppositeOf(Direction other) => DeltaRow == -other.DeltaRow && DeltaCol == -other.DeltaCol;
    }

    private sealed record Node(Cell Cell, DirectionHistory? History)
    {
        public override string ToString() => $"Node({Cell}, {History})";
    }

    private sealed record DirectionHistory(Direction Direction, int Count)
    {
        public override string ToString() => $"History({Direction}:{Count})";
    }

    private sealed record Cell(int Row, int Col)
    {
        public override string ToString() => $"Cell({Row}, {Col})";

        public Cell Step(Direction direction) => new(Row + direction.DeltaRow, Col + direction.DeltaCol);
    }

    private sealed class Grid(int[][] costs)
    {
        public int Cols { get; } = costs[0].Length;

        public int[][] Costs { get; } = costs;

        public int Rows { get; } = costs.Length;

        public bool IsInside(Cell cell) => cell.Row >= 0 && cell.Row < Rows && cell.Col >= 0 && cell.Col < Cols;
    }
}
